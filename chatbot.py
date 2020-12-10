import nltk
#nltk.download('punkt')
#run this command in python console to download punkt
from nltk.stem.lancaster import LancasterStemmer
stemmer = LancasterStemmer()

import numpy
import tensorflow as tf
from tensorflow import keras 
import random
import json

with open("intents.json") as file:
    data = json.load(file)
    
words = []
labels = []
docs_x = []
docs_y = []

for intent in data["intents"]:
    for pattern in intent["patterns"]:
        wrds = nltk.word_tokenize(pattern)
        words.extend(wrds)
        docs_x.append(wrds)
        docs_y.append(intent["tag"])

    if intent["tag"] not in labels:
        labels.append(intent["tag"])

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

#----------------------------------------------------------------------
#creating the neural net
model = tf.keras.Sequential()

model.add(tf.keras.layers.InputLayer(input_shape=(len(training[0]))))
model.add(tf.keras.layers.Dense(8))
model.add(tf.keras.layers.Dense(8))
model.add(tf.keras.layers.Dense(8))
model.add(tf.keras.layers.Dense(len(output[0]), activation="softmax"))

#run this command to get the summary of the model
#model.summary()

#----------------------------------------------------------------------

def train():
    model.compile(optimizer="adam", loss="categorical_crossentropy", metrics=["accuracy"])
    model.fit(training, output, epochs=500, batch_size=256)
    model.save('model.h5')

try:
    model = keras.models.load_model('model.h5')
except:
    train()
    
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
        print("Start talking with the bot (type quit to stop and retrain to train again)!")
        while True:
            inp = input("You: ")
            if inp.lower() == "/quit":
                break
                exit()
            elif inp.lower() == "/retrain":
                train()
                chat()
            else:
                results = model.predict([bag_of_words(inp, words)])[0]
               
                results_index = numpy.argmax(results)
                tag = labels[results_index]

                if results[results_index] > 0.9:
                    for tg in data["intents"]:
                        if tg["tag"] == tag:
                            responses = tg["responses"]

                    print(f"{random.choice(responses)}   (Category: {tag})")

                else:
                    print("Please rephrase it!") 
                    try:
                        with open('exceptions.txt') as f:
                            if inp not in f.read():
                                with open('exception.txt', 'a') as f:
                                    f.write(f'{inp}  (Predicted category: {tag})\n')
                    except:
                        file = open('exception.txt', 'x'):
                        with open('exception.txt') as f:
                            if inp not in f.read():
                                with open('exception.txt', 'a') as f:
                                    f.write(f'{inp}  (Predicted category: {tag})\n')
                                    
chat()                
                       
         


                  
                                















