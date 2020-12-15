#------------------------#
# Tensorflow 2.0 chatbot #
#   by VISHANK           #
#------------------------#


#nltk.download('punkt')
#run this command in python console to download punkt

import numpy
import tensorflow as tf	import tensorflow as tf
from tensorflow import keras 	from tensorflow import keras 
import random	import random
import json	import json
import nltk	import nltk
from nltk.stem.lancaster import LancasterStemmer	from nltk.stem.lancaster import LancasterStemmer
with open("intents.json") as file:	with open("intents.json") as file:
    data = json.load(file)	    data = json.load(file)
stemmer = LancasterStemmer()    	stemmer = LancasterStemmer()    
    	    
words = []	words = []
labels = []	labels = []
docs_x = []	docs_x = []
docs_y = []	docs_y = []
for intent in data["intents"]:	for intent in data["intents"]:
    for pattern in intent["patterns"]:	    for pattern in intent["patterns"]:
        wrds = nltk.word_tokenize(pattern)	        wrds = nltk.word_tokenize(pattern)
        words.extend(wrds)	        words.extend(wrds)
        docs_x.append(wrds)	        docs_x.append(wrds)
        docs_y.append(intent["tag"])	        docs_y.append(intent["tag"])
    if intent["tag"] not in labels:	    if intent["tag"] not in labels:
        labels.append(intent["tag"])	        labels.append(intent["tag"])
words = [stemmer.stem(w.lower()) for w in words if w != ("?" or "!")]	words = [stemmer.stem(w.lower()) for w in words if w != ("?" or "!")]
words = sorted(list(set(words)))	words = sorted(list(set(words)))
labels = sorted(labels)	labels = sorted(labels)
training = []	training = []
output = []	output = []
out_empty = [0 for _ in range(len(labels))]	out_empty = [0 for _ in range(len(labels))]
for x, doc in enumerate(docs_x):	for x, doc in enumerate(docs_x):
    bag = []	    bag = []
    wrds = [stemmer.stem(w.lower()) for w in doc]	    wrds = [stemmer.stem(w.lower()) for w in doc]
    for w in words:	    for w in words:
        if w in wrds:	        if w in wrds:
            bag.append(1)	            bag.append(1)
        else:	        else:
            bag.append(0)	            bag.append(0)
    output_row = out_empty[:]	    output_row = out_empty[:]
    output_row[labels.index(docs_y[x])] = 1	    output_row[labels.index(docs_y[x])] = 1
    training.append(bag)	    training.append(bag)
    output.append(output_row)	    output.append(output_row)
training = numpy.array(training)	training = numpy.array(training)
output = numpy.array(output)	output = numpy.array(output)
#----------------------------------------------------------------------	#----------------------------------------------------------------------
#creating the neural net	#creating the neural net
model = tf.keras.Sequential()	model = tf.keras.Sequential()
model.add(tf.keras.layers.InputLayer(input_shape=(len(training[0]))))	model.add(tf.keras.layers.InputLayer(input_shape=(len(training[0]))))
model.add(tf.keras.layers.Dense(8))	model.add(tf.keras.layers.Dense(8))
model.add(tf.keras.layers.Dense(8))	model.add(tf.keras.layers.Dense(8))
model.add(tf.keras.layers.Dense(8))	model.add(tf.keras.layers.Dense(8))
model.add(tf.keras.layers.Dense(len(output[0]), activation="softmax"))	model.add(tf.keras.layers.Dense(len(output[0]), activation="softmax"))
#run this command to get the summary of the model	#run this command to get the summary of the model
#model.summary()	#model.summary()
#----------------------------------------------------------------------	#----------------------------------------------------------------------
def train():	def train():
    model.compile(optimizer="adam", loss="categorical_crossentropy", metrics=["accuracy"])	    model.compile(optimizer="adam", loss="categorical_crossentropy", metrics=["accuracy"])
    model.fit(training, output, epochs=500, batch_size=256)	    model.fit(training, output, epochs=500, batch_size=256)
    model.save('model.h5')	    model.save('model.h5')
try:	try:
    model = keras.models.load_model('model.h5')	    model = keras.models.load_model('model.h5')
except:	except:
    train()	    train()
    	    
def bag_of_words(s, words):	def bag_of_words(s, words):
    bag = [0 for _ in range(len(words))]	    bag = [0 for _ in range(len(words))]
    s_words = nltk.word_tokenize(s)	    s_words = nltk.word_tokenize(s)
    s_words = [stemmer.stem(word.lower()) for word in s_words]	    s_words = [stemmer.stem(word.lower()) for word in s_words]
    for se in s_words:	    for se in s_words:
        for i, w in enumerate(words):	        for i, w in enumerate(words):
            if w == se:	            if w == se:
                bag[i] = 1	                bag[i] = 1
                	                
    return numpy.array([bag])	    return numpy.array([bag])
  	  
def chat():	def chat():
        print("Start talking with the bot (type quit to stop and retrain to train again)!")	        print("Start talking with the bot (type quit to stop and retrain to train again)!")
        while True:	        while True:
            inp = input("You: ")	            inp = input("You: ")
            if inp.lower() == "/quit":	            if inp.lower() == "/quit":
                break	                break
                exit()	                exit()
            elif inp.lower() == "/retrain":	            elif inp.lower() == "/retrain":
                train()	                train()
                chat()	                chat()
            else:	            else:
                results = model.predict([bag_of_words(inp, words)])[0]	                results = model.predict([bag_of_words(inp, words)])[0]
               	               
                results_index = numpy.argmax(results)	                results_index = numpy.argmax(results)
                tag = labels[results_index]	                tag = labels[results_index]
                if results[results_index] > 0.9:	                if results[results_index] > 0.9:
                    for tg in data["intents"]:	                    for tg in data["intents"]:
                        if tg["tag"] == tag:	                        if tg["tag"] == tag:
                            responses = tg["responses"]	                            responses = tg["responses"]
                    print(f"{random.choice(responses)}   (Category: {tag})")	                    print(f"{random.choice(responses)}   (Category: {tag})")
                else:	                else:
                    print("Please rephrase it!") 	                    print("Please rephrase it!") 
                    try:	                    try:
                        with open('exceptions.txt') as f:	                        with open('exceptions.txt') as f:
                            if inp not in f.read():	                            if inp not in f.read():
                                with open('exceptions.txt', 'a') as f:	                                with open('exceptions.txt', 'a') as f:
                                    f.write(f'{inp}  (Predicted category: {tag})\n')	                                    f.write(f'{inp}  (Predicted category: {tag})\n')
                    except:	                    except:
                        file = open('exceptions.txt', 'x'):	                        file = open('exceptions.txt', 'x'):
                        with open('exceptions.txt') as f:	                        with open('exceptions.txt') as f:
                            if inp not in f.read():	                            if inp not in f.read():
                                with open('exceptions.txt', 'a') as f:	                                with open('exceptions.txt', 'a') as f:
                                    f.write(f'{inp}  (Predicted category: {tag})\n')	                                    f.write(f'{inp}  (Predicted category: {tag}, accuracy: {results[results_index]})\n')


chat()                	chat()
