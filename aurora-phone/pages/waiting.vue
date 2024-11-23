<template>
    <div>
  
      <div class="waiting-room">
          <h1 class="waiting-room__cb-title"> Waiting for game to start <span class="cb-yellow">...</span> </h1>
      </div>
      <button @click="test()">test start game</button>
    </div>
  </template>
  
<script lang="js">

export default defineComponent({
   name: 'WaitScren',
  setup() {
    const rc = useState("roomCode");

    onMounted(async () => {
      alert(rc.value);
    });
  },
  methods: {
    test() {
      navigateTo('/game');
    }
  }
})
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
  
  .waiting-room {
    height: 95vh;
    width: 100vw;
    background: linear-gradient(20deg, rgb(255, 255, 255) 46.9%, rgb(54, 54, 54) 46%);
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
  