<template>
  <div>
    <div class="waiting-room">
      <h1 class="waiting-room__cb-title">
        Waiting for game to start <span class="cb-yellow">...</span>
      </h1>
    </div>
  </div>
</template>

<script lang="ts" setup>
import * as signalR from "@microsoft/signalr";
const { $toast } = useNuxtApp();
const runtimeConfig = useRuntimeConfig();
const roomCode = useState<string>("roomCode");
if (roomCode.value === "") {
  navigateTo({ path: "/" });
}

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${runtimeConfig.public.apiBase}/ws/game`, {
    withCredentials: false, // This is important, otherwise you get CORS errors
  })
  .build();

connection.on("GameStartedEvent", (dto) => {
  if (dto.roomCode === roomCode.value) {
    navigateTo({ path: "/game" });
  } else {
    $toast.error("Cannot add you to lobby");
  }
});

connection.start().catch((err) => console.error(err.toString()));
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap");

html,
body {
  overscroll-behavior: none;
}

html {
  font-family: "Montserrat", sans-serif;
  font-size: large;
}

body {
  margin: 0;
  padding: 0;
}

.waiting-room {
  height: 95vh;
  width: 100vw;
  background: linear-gradient(
    20deg,
    rgb(255, 255, 255) 46.9%,
    rgb(54, 54, 54) 46%
  );
  background-size: 100% 200%; /* Allows room for animation */
  animation: gradient-move 2.5s infinite alternate;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

@keyframes gradient-move {
  0% {
    background-position: top;
  }
  100% {
    background-position: bottom;
  }
}

.waiting-room__cb {
  display: flex;
  justify-content: center;

  &-title {
    color: #000;
    width: 80%;
  }
}
</style>
