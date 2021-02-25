import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Register from "../views/Register.vue";
import Login from "../views/Login.vue";
import Photo from "../views/Photo.vue";
import PhotoShare from "../views/Share/PhotoShare.vue";
import Share from "../views/Share.vue";

Vue.use(VueRouter);

// var nameDirectoryPath = this.$store.state.atDirectory
const routes = [
  {
    path: "/",
    name: "Home",
    component: Home
  },
  {
    path: "/MyDrive/:id?",
    name: "About",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/Home.vue")
  },
  {
    path: "/register",
    name: "Register",
    component: Register
  },
  {
    path: "/login",
    name: "Login",
    component: Login
  },
  {
    path: "/Preview/:path?",
    name: "Preview",
    component: Photo
  },
  {
    path: "/Share/:id?",
    name: "Share",
    component: Share
  },
  {
    path: "/PreviewShare/:path?",
    name: "PreviewShare",
    component: PhotoShare
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
