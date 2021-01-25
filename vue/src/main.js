import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import VueModal from "@kouts/vue-modal";
import "@kouts/vue-modal/dist/vue-modal.css";

import axios from "axios";
import store from "./store";


Vue.config.productionTip = false;
Vue.component("Modal", VueModal);
const axiosInstance = axios.create({
  baseURL: "http://localhost:5000/",
  timeout: 1000,
  headers: { "X-Custom-Header": "foobar", "Content-Type": "application/json" },
});
Vue.prototype.$axios = axiosInstance;

// Vue.use(axios);
// axios.defaults.baseURL = 'http://localhost:5000';

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
