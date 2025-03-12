<script setup lang="ts">
import { ref } from 'vue';

export interface Weather {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

const weatherForecasts = ref<Weather[]>([]);

async function fetchWeatherForecasts() {
  const res = await fetch('/api/weatherforecast');
  const data = await res.json() as Weather[];
  weatherForecasts.value = data;
}
fetchWeatherForecasts();

</script>

<template>
  <h2>Weather Forecasts</h2>
  <button @click="fetchWeatherForecasts">Refresh Forecasts</button>
  <div>Server caches for 3 seconds</div>
  <table class="table">
    <thead>
      <tr>
        <th>Date</th>
        <th>Temperature</th>
        <th>Summary</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="weather in weatherForecasts" :key="weather.date">
        <td>{{ weather.date }}</td>
        <td>{{ weather.temperatureC }}</td>
        <td>{{ weather.summary }}</td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped>
.table {
  width: 100%;
}
</style>
