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

def createNewModel(name, num_epochs, batch_size_val, learning_rate_val, hidden_layers):
    model = tf.keras.Sequential()
    model.add(tf.keras.layers.InputLayer(input_shape=(len(training[0]))))
    #I might remove this depending on if we can transfer an array or not from UI
    for layer in hidden_layers:
        if layer.type == "dense":
            model.add(tf.keras.layers.Dense(8))
        elif layer.type == "flatten":
            model.add(tf.keras.layers.Flatten())
    model.add(tf.keras.layers.Dense(8))
    model.add(tf.keras.layers.Dense(8))
    model.add(tf.keras.layers.Dense(8))
    model.add(tf.keras.layers.Dense(len(output[0]), activation="softmax"))

    train(model, name, num_epochs, batch_size_val, learning_rate_val)
    # run this command to get the summary of the model
    # model.summary()

# ----------------------------------------------------------------------

#epoch = 10000  batch size = 1000 optimiser = "adam"
def train(model, name, num_epochs, batch_size_val, learning_rate_val):
    #sets the learning rate for the adam optimizer
    opt = keras.optimizers.Adam(learning_rate = learning_rate_val)
    model.compile(optimizer=opt,
                  loss="categorical_crossentropy", metrics=["accuracy"])
    model.fit(training, output, epochs=num_epochs, batch_size=batch_size_val)
    model.save('KerasModels\\' + name + '.h5')

def loadModel(model_name):
    try:
        model = keras.models.load_model('KerasModels\\' + model_name + '.h5')
        return model
    except:
        #model not found exception
        train()

#The current active model (pass in the name from the UI)
current_model = loadModel(current_model_name)


def bag_of_words(s, words):
    bag = [0 for _ in range(len(words))]

    s_words = nltk.word_tokenize(s)
    s_words = [stemmer.stem(word.lower()) for word in s_words]

    for se in s_words:
        for i, w in enumerate(words):
            if w == se:
                bag[i] = 1

    return numpy.array([bag])


def chat():
    print("Start talking with the bot (type /quit to stop and /retrain to train again)!")
    while True:
        inp = input("You: ")

        if inp.lower() == "/quit":
            break
            exit()

        elif inp.lower() == "/retrain":
            train()
            chat()

        else:
            results = current_model.predict([bag_of_words(inp, words)])[0]

            results_index = numpy.argmax(results)
            intent = labels[results_index]
            if results[results_index] > 0.9:
                for tg in data["intents"]:
                    if tg["intent"] == intent:
                        responses = tg["responses"]
                print(f"{random.choice(responses)}   (Category: {intent})")

            else:
                print("Please rephrase it!")
                try:
                    with open('exceptions.txt') as f:
                        if inp not in f.read():
                            with open('exceptions.txt', 'a') as f:
                                f.write(
                                    f'{inp}  (Predicted category: {intent})\n')
                except:
                    file = open('exceptions.txt', 'x')
                    with open('exceptions.txt') as f:
                        if inp not in f.read():
                            with open('exceptions.txt', 'a') as f:
                                f.write(
                                    f'{inp}  (Predicted category: {intent})\n')


chat()
