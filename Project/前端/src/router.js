import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";

Vue.use(Router);

export default new Router({
  mode: "history",
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/",
      name: "home",
      component: Home
    },
    {
      path: "/detail/:id",
      name: "detail",
      component: () => import("./views/Detail.vue")
    },
    
    {
      path: "/cart",
      name: "cart",
      component: () => import("./views/Cart.vue")
    },
    {
      path: "/order",
      name: "order",
      component: () => import("./views/Order.vue")
    },
    {
      path: "/my",
      name: "my",
      component: () => import("./views/My.vue")
    },
    {
      path: "/admin",
      name: "admin",
      component: () => import("./views/Admin.vue")
    }
  ]
});
