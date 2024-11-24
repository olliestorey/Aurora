<template>
  <div class="game">
    <div class="game__word-container">
      <h1 class="game__word cb-yellow">{{ gameCurrentTitle }}</h1>
      <div class="game__word-entered">
        <input type="text" :placeholder="currentPlayerWord" disabled />
      </div>
    </div>

    <div class="game__letter-height">
      <div class="game__letter-container">
        <button
          v-for="(letter, index) in gameCurrentScrambleTile"
          :key="index"
          class="game__letter"
          @click="addLetter(letter, $event)"
        >
          {{ letter }}
        </button>
      </div>
    </div>
    <button class="game__clear" @click="clearEntry()">Clear</button>
    <!-- <button class="game_pass" @click="skipWord()">Skip</button> -->
  </div>
</template>
<script lang="js">
 import { defineComponent, ref, onMounted } from 'vue';
 import { useNuxtApp } from '#app';

 const { $toast } = useNuxtApp();

const runtimeConfig = useRuntimeConfig();

 export default defineComponent({
   name: 'GameScreen',
   setup() {
     const roomCode = useState("roomCode");
     const playerKey = useState("playerKey");

     const fullWordList = ref([]);

     let currentPlayerWord = ref("");
     const currentAnagramWord = ref("");
     const currentIndex = ref(0);
     const gameCurrentScrambleTile = ref("");
     const gameCurrentTitle = ref("");

    const fetchWordlist = async () => {
      const response = await fetch(`${runtimeConfig.public.apiBase}/api/room?roomCode=${roomCode.value}`)
      const data = await response.json()
      fullWordList.value = data.words; // Update fullWordList with the fetched words
    };

    const scrambleWord = (word) => {
      let scrambled = word.split('').sort(() => Math.random() - 0.5).join('');
      if (scrambled === word) {
        return scrambleWord(word);
      }
      return scrambled;
    };

    const setCurrentAnagramWord = () => {
      const word = fullWordList.value[currentIndex.value];
      currentAnagramWord.value = word;
      gameCurrentScrambleTile.value = scrambleWord(word);
      gameCurrentTitle.value = scrambleWord(word);

      // Reset all letter classes
      document.querySelectorAll('.game__letter').forEach((letter) => {
        letter.classList.remove('game__letter--used');
      });
    };

    const clearEntry = () => {
      currentPlayerWord.value = "";
      document.querySelectorAll('.game__letter').forEach((letter) => {
        letter.classList.remove('game__letter--used');
      })
    };

     const scrambledTitle = (word) => {
      //TODO - Keep spaces consistent
       return scrambleWord(word);
     };

     // Add letter to the current word
     const addLetter = (letter, event) => {
       event.target.classList.add('game__letter--used');
       currentPlayerWord.value += letter;
       checkInputs();
     };

    const submitWord = async (word) => {
      const response = await fetch(
        `${runtimeConfig.public.apiBase}/api/words/submitWord`,
        {
          headers: {
            "Content-Type": "application/json",
          },
          method: "POST",
          body: JSON.stringify({
            roomCode: roomCode.value,
            playerKey: playerKey.value,
            word: word,
          }),
        }
      );

      return await response.json();
    }

     // Check if the player's input is correct
     const checkInputs = async () => {
       if (
         currentPlayerWord.value &&
         currentAnagramWord.value &&
         currentPlayerWord.value.length === currentAnagramWord.value.length
       ) {
         // All letters have been entered
         if (currentPlayerWord.value === currentAnagramWord.value) {
          let response = await submitWord(currentPlayerWord.value);
          if (response.result) {
            if (response.position !== null){
              $toast.success('Game Complete');
                useState(
                "playerPosition",
                () => response.position
                );
              await navigateTo({ path: "/results" });
            } else {
              $toast.success('Correct Word!');
              currentIndex.value++;
              currentPlayerWord.value = "";
              setCurrentAnagramWord();
            }
          } else {
            $toast.error('Duplicate Word!');
          }

         } else {
           $toast.error('Incorrect Word!');
           currentPlayerWord.value = "";
           document.querySelectorAll('.game__letter').forEach((letter) => {
             letter.classList.remove('game__letter--used');
           });
         }
       }
     };

     onMounted(async () => {
       await fetchWordlist();
       setCurrentAnagramWord();
     });

     // Return everything needed for the template to access
     return {
       currentPlayerWord,
       currentAnagramWord,
       currentIndex,
       gameCurrentScrambleTile,
       gameCurrentTitle,
       fullWordList,
       scrambleWord,
       setCurrentAnagramWord,
       scrambledTitle,
       addLetter,
       checkInputs,
       clearEntry,
       roomCode, // Make sure to return roomCode to use in the template
     };
   },
 });
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap");

html {
  font-family: "Montserrat", sans-serif;
  font-size: large;
}

body {
  margin: 0;
  padding: 0;
}

.game {
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
