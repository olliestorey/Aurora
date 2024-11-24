<template>
  <div>
    <pre>{{ runtimeConfig }}</pre>
  </div>
</template>

<script setup>
const runtimeConfig = useRuntimeConfig();
</script>
