<script setup lang="ts">
import { ref } from 'vue';

// Thanks https://github.com/meteostat/weather-stations

export interface WeatherStation {
  id: string;
  name: string;
  timezone: string;
  country: string;
  latitude: number;
  longitude: number;
  elevation: number;
}

const weatherStations = ref<WeatherStation[]>([]);

async function fetchWeatherStations() {
  weatherStations.value = [{id:'loading...', name:'', timezone:'', country:'', latitude:0, longitude:0, elevation:0}];
  const res = await fetch('/api/weatherstations');
  const data = await res.json() as WeatherStation[];
  weatherStations.value = data;
}
fetchWeatherStations();

</script>

<template>
  <h2>Weather Stations</h2>
  <button @click="fetchWeatherStations">Refresh DB Call</button>
  <table class="table">
    <thead>
      <tr>
        <th>Id</th>
        <th>Name</th>
        <th>Country</th>
        <td>Timezone</td>
        <td>Location</td>
        <td>Elevation (meters)</td>
      </tr>
    </thead>
    <tbody>
      <tr v-for="weather in weatherStations" :key="weather.id">
        <td>{{ weather.id }}</td>
        <td>{{ weather.name }}</td>
        <td>{{ weather.country }}</td>
        <td>{{ weather.timezone }}</td>
        <td>{{ weather.latitude}} x {{ weather.longitude }}</td>
        <td>{{ weather.elevation }}</td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped>
.table {
  width: 100%;
}
</style>
