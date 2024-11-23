<template>
  <div>
    <h1>Lobby {{ roomCode }}</h1>
    <ul>
      <li v-for="player in playersInRoom" :key="player">
        {{ player }}
      </li>
    </ul>

    <button @click="startGameCountdown()">Start Game</button>
    <div v-if="gameStarting && countdown > 0">
      <h2 v-if="countdown > 0">Game starting in {{ countdown }}...</h2>
      <button @click="cancelStartGame()">Cancel Start</button>
    </div>
  </div>
</template>

<script lang="ts" setup>
import * as signalR from "@microsoft/signalr";
import { ref } from "vue";

const runtimeConfig = useRuntimeConfig();

let playersInRoom = ref<string[]>([]);
const roomCode = useState<string>("roomCode"); //TODO: get this from global state
let countdown = ref<number>(5);
let gameStarting = ref<boolean>(false);

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${runtimeConfig.public.apiBase}/ws/player`, {
    withCredentials: false, // This is important, otherwise you get CORS errors
  })
  .build();

connection.on("PlayerJoinedGameEvent", (dto) => {
  playersInRoom.value.push(dto.playerName);
});

connection.start().catch((err) => console.error(err.toString()));

const rconnection = new signalR.HubConnectionBuilder()
  .withUrl(`${runtimeConfig.public.apiBase}/ws/room`, {
    withCredentials: false, // This is important, otherwise you get CORS errors
  })
  .build();

rconnection.on("GameStartedEvent", (dto) => {
  navigateTo({ path: "/game-leaderboard" });
});

rconnection.start().catch((err) => console.error(err.toString()));

// Timeout duration (5 seconds)
const timeoutDuration = countdown.value * 1000;

let interval: ReturnType<typeof setTimeout> | null = null;
let startGameTimeout: ReturnType<typeof setTimeout> | null = null;

async function startGameCountdown() {
  gameStarting.value = true;
  interval = setInterval(() => (countdown.value -= 1), 1000);
  startGameTimeout = setTimeout(async () => {
    await startGame();
  }, timeoutDuration);
}

async function startGame() {
  const response = await fetch(
    `${runtimeConfig.public.apiBase}/api/room/startgame`,
    {
      method: "POST",
      body: JSON.stringify({
        roomCode: roomCode.value,
      }),
    }
  );
}

function cancelStartGame() {
  if (startGameTimeout !== null) {
    clearTimeout(startGameTimeout);
    startGameTimeout = null;
  }
  if (interval !== null) {
    clearInterval(interval);
    interval = null;
  }
  countdown.value = 5;
  gameStarting.value = false;
}
</script>

<style lang="scss" scoped>
html {
  font-family: "Montserrat", sans-serif;
  font-size: large;

  &:has(.leaderboard-global) {
    background-color: rgb(54, 54, 54);
  }
}

body {
  margin: 0;
  padding: 0;
}

.leaderboard-global {
  display: flex;
  color: white;
  background: rgb(54, 54, 54);
  margin: 0;
  border-bottom: 4px solid #e6c300;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 20px;
  position: sticky;
  top: 0;
  z-index: 1000;

  &__entry {
    background-color: white;
    margin: 0;
    padding: 10px;
    border-radius: 50px;
    display: flex;
    justify-content: space-between;
    align-items: center;

    &-position {
      padding: 10px;
      border-radius: 100%;
      width: 18px;
      height: 18px;
      text-align: center;
      line-height: 18px;
      font-weight: bolder;
    }

    &-score {
      padding: 10px;
      border-radius: 50px;
      width: min-content;
      height: 18px;
      text-align: center;
      line-height: 18px;
      font-weight: bolder;
    }

    &-name {
      line-height: 40px;
      text-align: center;
    }
  }

  &__details {
    display: flex;
    gap: 20px;
    width: min-content;
  }

  &__results {
    display: flex;
    flex-direction: column;
    gap: 4px;
    padding: 10px;

    background: rgb(54, 54, 54);
  }

  &__title {
    margin: 0;
    padding-top: 16px;
  }
}

.cb {
  &-black {
    color: #171717;
  }

  &-yellow {
    color: #e6c300;

    &--bg {
      background-color: #e6c300;
    }
  }
}
</style>
