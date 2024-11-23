<template>
  <div>
    <h1>Room {{ roomCode }}</h1>
    <button @click="regenRoomCode()">Regeneratate</button>
    Number of words
    <input type="number" v-model="numberOfWordsInGame" />
    <button @click="createRoom()">Create Room</button>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";

const roomCode = useState<string>("roomCode", () => genCode());
const numberOfWordsInGame = ref<number>(1);
const runtimeConfig = useRuntimeConfig();

function regenRoomCode() {
  roomCode.value = genCode();
}

function genCode() {
  const characters =
    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  let code = "";
  for (let i = 0; i < 5; i++) {
    const randomIndex = Math.floor(Math.random() * characters.length);
    code += characters[randomIndex];
  }
  return code;
}

async function createRoom() {
  const response = await fetch(
    `${runtimeConfig.public.apiBase}/api/room/create`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        roomCode: roomCode.value,
        numberOfWordsInGame: numberOfWordsInGame.value,
      }),
    }
  );

  if (response.ok) {
    console.log(`Room ${roomCode.value} created`);
    await navigateTo({ path: "/lobby" });
  } else {
    console.error("Failed to create room");
  }
}
</script>
