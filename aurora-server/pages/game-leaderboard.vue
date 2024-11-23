<template>
  <div>
    <h1>Leaderboard {{ roomCode }}</h1>
  </div>
</template>

<script lang="ts" setup>
import * as signalR from "@microsoft/signalr";
import { ref } from "vue";

const runtimeConfig = useRuntimeConfig();

let playersInRoom = ref<string[]>([]);
const roomCode = useState<string>("roomCode");
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
