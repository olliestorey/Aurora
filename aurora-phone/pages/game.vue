<template>
    <div class="game">
      <div class="game__word-container">
        <h1 class="game__word cb-yellow"> {{ scrambledTitle }} </h1>
        <div class="game__word-entered">
            <input type="text" placeholder="" disabled/>
          </div>
      </div>

      <div class="game__letter-height">
          <div class="game__letter-container">
          <button v-for="i in scrambledTiles" :key="i" class="game__letter"> {{ i }} </button>
          </div>
      </div>
    </div>
  </template>

  <script lang="ts">
  import { defineComponent } from 'vue';
  
  export default defineComponent({
    name: 'GameScreen',
    props: {
      fullWordList: {
        type: Array as () => string[],
        default: ['ollie', 'jess', 'niz', 'win', 'hackathon'],
      },
      currentAnagramWord: {
        type: String as () => string,
        default: 'im a long word',
      },
    },
    computed: {
      scrambledTitle(): string {
        let scrambled: string;
        do {
          scrambled = this.scrambleWord(this.currentAnagramWord);
        } while (scrambled === this.currentAnagramWord);
        return scrambled;
      },
  
      scrambledTiles(): string {
        let scrambled: string;
        do {
          scrambled = this.currentAnagramWord.split('').sort(() => Math.random() - 0.5).join('');
        } while (scrambled === this.currentAnagramWord);
        return scrambled;
      },
    },
    mounted() {
      console.log(this.fullWordList);
      console.log(this.currentAnagramWord);
    },
    methods: {
      scrambleWord(word: string): string {
        const chars = word.split('');
        const letters = chars.filter(char => char !== ' ').sort(() => Math.random() - 0.5);
        return chars.map(char => (char === ' ' ? ' ' : letters.shift()!)).join('');
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
  