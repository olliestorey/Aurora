<template>
    <div class="game">
      <div class="game__word-container">
        <h1 class="game__word cb-yellow"> {{ gameCurrentTitle }} </h1>
        <div class="game__word-entered">
            <input type="text" :placeholder="currentPlayerWord" disabled />
          </div>
      </div>

      <div class="game__letter-height">
          <div class="game__letter-container">
          <button v-for="(letter, index) in gameCurrentScrambleTile" :key="index" class="game__letter" @click="addLetter(letter, $event)"> {{ letter }} </button>
          </div>
      </div>
    </div>
  </template>
 
 <script lang="js">
 import { defineComponent } from 'vue';
 import { useNuxtApp } from '#app';
 
 export default defineComponent({
   name: 'GameScreen',
   data() {
     return {
       currentPlayerWord: "",
       currentAnagramWord: "",
       currentIndex: 0,
 
       gameCurrentScrambleTile: "",
       gameCurrentTitle: "",
     };
   },
   props: {
     fullWordList: {
       type: Array,
       default: () => ['ollie', 'jess', 'niz', 'win', 'hackathon'], // Default word list
     },
   },
   mounted() {
     this.setCurrentAnagramWord();
   },
   methods: {
     // Method to scramble a word
     scrambleWord(word) {
       let scrambled = word.split('').sort(() => Math.random() - 0.5).join('');
       if (scrambled === word) {
         return this.scrambleWord(word);
       }
       return scrambled;
     },
     // Set the current anagram and scramble the word
     setCurrentAnagramWord() {
       const word = this.fullWordList[this.currentIndex];
       this.currentAnagramWord = word;
       this.gameCurrentScrambleTile = this.scrambleWord(word);
       this.gameCurrentTitle = this.scrambleWord(word);
 
       // Reset all letter classes
       document.querySelectorAll('.game__letter').forEach((letter) => {
         letter.classList.remove('game__letter--used');
       });
     },
     // Scramble a word
     scrambledTitle(word) {
       return this.scrambleWord(word);
     },
     // Add letter to the current word
     addLetter(letter, event) {
       event.target.classList.add('game__letter--used');
       this.currentPlayerWord += letter;
       this.checkInputs();
     },
     // Check if the player's input is correct
     checkInputs() {
       if (
         this.currentPlayerWord &&
         this.currentAnagramWord &&
         this.currentPlayerWord.length === this.currentAnagramWord.length
       ) {
         // All letters have been entered
         if (this.currentPlayerWord === this.currentAnagramWord) {
           this.$toast.success('Correct Word!'); // Correct word notification
           this.currentIndex++;
           this.currentPlayerWord = "";
           this.setCurrentAnagramWord();
         } else {
           this.$toast.error('Incorrect Word!'); // Incorrect word notification
           this.currentPlayerWord = "";
           this.setCurrentAnagramWord();
         }
       }
     },
   },
 });
 </script>
 
  
  
  <style lang="scss">
  @import url('https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap');
  
  html {
    font-family: 'Montserrat', sans-serif;
    font-size: large;
  }
  
  body {
    margin: 0;
    padding: 0;
  }
  
  .game{
    height: 95vh;
    width: 100vw;
    display: flex;
    flex-direction: column;
    gap: 20px;
  
    &__letter {
      cursor: pointer;
      font-weight: 700;
      text-transform: uppercase;
      width: calc(25% - 8px);
      padding: 0;
  
      &-container {
        display: flex;
        gap: 10px;
        padding: 20px;
        margin-top: 20px;
        flex-wrap: wrap;
      }

      &--used {
        pointer-events: none;
        background-color: grey;
        color: rgb(54, 54, 54);
      }
    }
  
    &__word {
      text-align: center;
      width: 100%;
      margin: 0;
      padding: 20px 0;
      background-color: rgb(54, 54, 54);
      margin-bottom: 20px;
  
      &-entered {
        width: 100%;
        display: flex;
        justify-content: center;
  
        input {
          font-size: 20px;
          letter-spacing: 2px;
          background-color: white;
          box-shadow: 0 10px 30px rgb(58 58 58 / 59%);
          border: 1px solid rgb(110, 110, 110);
        }
      }
  
      &-container {
        flex: 1;
      }
    }
  }
  </style>