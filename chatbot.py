#------------------------#
# Tensorflow 2.0 chatbot #
#   by VISHANK           #
#------------------------#


# nltk.download('punkt')
# run this command in python console to download punkt

import numpy
import tensorflow as tf
from tensorflow import keras
import random
import json
import nltk
from nltk.stem.lancaster import LancasterStemmer
import sys

invalid_responses = ["Please rephrase that.", "That is weird, I do not recognize that.", "Try again later.", "Could not come up with a response, try again."]


with open("intent.json") as file:
    data = json.load(file)

stemmer = LancasterStemmer()

words = []
labels = []
docs_x = []
docs_y = []

for intent in data["intents"]:
    for pattern in intent["patterns"]:
        wrds = nltk.word_tokenize(pattern)
        words.extend(wrds)
        docs_x.append(wrds)
        docs_y.append(intent["intent"])

    if intent["intent"] not in labels:
        labels.append(intent["intent"])

words = [stemmer.stem(w.lower()) for w in words if w != ("?" or "!")]
words = sorted(list(set(words)))
labels = sorted(labels)

training = []
output = []

out_empty = [0 for _ in range(len(labels))]

for x, doc in enumerate(docs_x):
    bag = []
    wrds = [stemmer.stem(w.lower()) for w in doc]

    for w in words:
        if w in wrds:
            bag.append(1)
        else:
            bag.append(0)

    output_row = out_empty[:]
    output_row[labels.index(docs_y[x])] = 1

    training.append(bag)
    output.append(output_row)


training = numpy.array(training)
output = numpy.array(output)

# ----------------------------------------------------------------------

# creating the neural net

def createNewModel(model_name, num_epochs, batch_size_val, learning_rate_val, hidden_layers):
    # Check if model already exists
    model = loadModel(model_name)
    if (type(model) is not str): return f"Model with name {model_name} already exists"

    model = tf.keras.Sequential()
    model.add(tf.keras.layers.InputLayer(input_shape=(len(training[0]),)))
    if (len(hidden_layers) > 0):
        # Passed in layers
        for layer in hidden_layers:
            if layer[0] == "dense":
                model.add(tf.keras.layers.Dense(layer[1]))
            elif layer[0] == "flatten":
                model.add(tf.keras.layers.Flatten(layer[1]))
            elif layer[0] == "dropout":
                model.add(tf.keras.layers.Dropout(rate=layer[1]))
    else:
        # Default layers
        model.add(tf.keras.layers.Dense(12))
        model.add(tf.keras.layers.Dense(12))
        model.add(tf.keras.layers.Dense(12))
        model.add(tf.keras.layers.Dense(12))
        model.add(tf.keras.layers.Dense(12))
        model.add(tf.keras.layers.Dense(12))
    model.add(tf.keras.layers.Dense(len(output[0]), activation="softmax"))

    trainNew(model, model_name, num_epochs, batch_size_val, learning_rate_val)
    # run this command to get the summary of the model
    # model.summary()

# ----------------------------------------------------------------------
#epoch = 1000  batch size = 100 optimiser = "adam" learning_rate = 0.001
def train(model_name, num_epochs, batch_size_val, learning_rate_val):
    # Valid model check, does it exist?
    model = loadModel(model_name)
    if (type(model) is str): return model

    #sets the learning rate for the adam optimizer
    opt = keras.optimizers.Adam(learning_rate = learning_rate_val)
    model.compile(optimizer=opt,
                  loss="categorical_crossentropy", metrics=["accuracy"])
    model.fit(training, output, epochs=num_epochs, batch_size=batch_size_val)
    model.save('KerasModels\\' + model_name + '.h5')

def trainNew(model, model_name, num_epochs, batch_size_val, learning_rate_val):
    #sets the learning rate for the adam optimizer
    opt = keras.optimizers.Adam(learning_rate = learning_rate_val)
    model.compile(optimizer=opt,
                  loss="categorical_crossentropy", metrics=["accuracy"])
    model.fit(training, output, epochs=num_epochs, batch_size=batch_size_val)
    model.save('KerasModels\\' + model_name + '.h5')

def loadModel(model_name):
    try:
        model = keras.models.load_model('KerasModels\\' + model_name + '.h5')
        return model
    except:
        #model not found exception
        return "model: " + model_name + " could not be found"


def bag_of_words(s, words):
    bag = [0 for _ in range(len(words))]

    s_words = nltk.word_tokenize(s)
    s_words = [stemmer.stem(word.lower()) for word in s_words]

    for se in s_words:
        for i, w in enumerate(words):
            if w == se:
                bag[i] = 1

    return numpy.array([bag])


def chat(model_name, user_input):
    # Valid model check, does it exist?
    model = loadModel(model_name)
    if (type(model) is str): return model
    
    results = model.predict([bag_of_words(user_input, words)])[0]

    results_index = numpy.argmax(results)
    intent = labels[results_index]
    if results[results_index] > 0.4:
        for tg in data["intents"]:
            if tg["intent"] == intent:
                responses = tg["responses"]
        return f"{random.choice(responses)}"
    else:
        return f"{random.choice(invalid_responses)}"


#This is a test to make a new model
#hiddenlayers = [("dense", 8), ("dense", 8), ("dense", 8)]
#train("bob", 500, 50, 0.001)
#The current active model (pass in the name from the UI)
#print(chat("bob", "Hello"))
try:
    if (sys.argv[1] == "chat"):
        # Looking to chat, check if we have the right ammount of arguments

        if (len(sys.argv) >= 4):
            print(chat(sys.argv[2], " ".join(sys.argv[3:])))
        else:
            print("To few arugments were passed for function chat")
    elif (sys.argv[1] == "new_model"):
        # Looking to create a new model, did we pass hidden layers or not?
        if (len(sys.argv) == 6):
            print(createNewModel(sys.argv[2], sys.argv[3], sys.argv[4], sys.argv[5], []))
        elif (len(sys.argv) >= 8):
            # Convert the hidden layer strings to tuples for easy use in adding them to the model
            full_tuple = sys.argv[6:]
            first_half = full_tuple[:int(len(full_tuple)/2)]
            last_half = full_tuple[int(len(full_tuple)/2):]
            real_tuple = []
            # Combine both halves into a single tuple that gets appended
            for (first, last) in zip(first_half, last_half):
                real_tuple.append((first, last))
            print(createNewModel(sys.argv[2], sys.argv[3], sys.argv[4], sys.argv[5], real_tuple))
        else:
            print("To few arugments were passed for function createNewModel")
    elif (sys.argv[1] == "train"):
        # Looking to train a model, check if we have the right ammount of arguments
        if (len(sys.argv) == 6):
            print(train(sys.argv[2], sys.argv[3], sys.argv[4], sys.argv[5]))
        else:
            print("To few or to many arugments were passed for function train")
    else:
        # Unable to recognize the inputted function request identifier
        print(f"{sys.argv[1]} is not a valid function in chatbot.")

        ##This is a test to make a new model
    
        ###The current active model (pass in the name from the UI)
        #print(chat("bob", "Hello"))
except:
    try:
        userinput = input("Enter message:\n")

        while(userinput != "exit"):
            #hiddenlayers = ["dense", "dense", "dense"]
            #createNewModel("bob", 500, 50, 0.001, hiddenlayers)
            print(chat("bob", userinput))
            userinput = input("Enter message:\n")
    except :
        pass

    
   




