import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import ElementUI from "element-ui";
import "element-ui/lib/theme-chalk/index.css";
import axios from "axios";

Vue.config.productionTip = false;
Vue.use(ElementUI);
Vue.prototype.$axios = axios;
Vue.prototype.serverUrl = "http://localhost:81";
// Vue.prototype.serverUrl = "http://192.168.36.1:54971";
// Vue.prototype.serverUrl = "http://192.168.1.87:54971";



new Vue({
  router,
  render: h => h(App)
}).$mount("#app");
