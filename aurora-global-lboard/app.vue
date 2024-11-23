<template>
  <div>
    <h1>Leaderboard</h1>
    <ul>
      <li v-for="entry in leaderboard.entries" :key="entry.position">
        {{ entry.position }}. {{ entry.name }} - {{ entry.score }}
      </li>
    </ul>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";

type LeaderboardEntry = {
  name: string;
  score: number;
  position: number;
};

type Leaderboard = {
  entries: LeaderboardEntry[];
};

const runtimeConfig = useRuntimeConfig();

let leaderboard = ref<Leaderboard>({ entries: [] });

async function useFetch(url: string) {
  const response = await fetch(url);
  return await response.json();
}

onMounted(async () => {
  const response = await useFetch(
    `${runtimeConfig.public.apiBase}/api/globalleaderboard`
  );
  leaderboard = response;
});
</script>
