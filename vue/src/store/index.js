import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    path:"",
    allDataFile:[
      {

      }
    ],
    dataFile:[
      
    ],
    defaultDataFile:[],
    directory: ["Home"],
    atDirectory:"",
    deDirectory:"",
    User:{
      id:null,
      username:null,
    },
  },
  mutations: {},
  actions: {},
  modules: {}
});
