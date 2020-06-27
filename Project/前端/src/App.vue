<template>
  <div id="app">

    <header class="sticky">
      <div class="logo">LeeMall</div>
      <ul>
        <li><router-link to="/">购物大厅 /</router-link> </li>
        <li><router-link to="/cart">购物车 /</router-link> </li>
        <li><router-link to="/order">我的订单 /</router-link> </li>
        <li><router-link to="/my">个人中心 /</router-link></li>
        <li><a v-if="!isLogin" type="primary" size="mini" plain @click="handleLogin">登录</a>
          <a v-else type="primary" size="mini" plain @click="handleLogout">登出</a>
        </li>
      </ul>
    </header>
    <router-view/>
    <!-- 登录对话框 -->
    <el-dialog title="登录" :visible.sync="loginFormVisible" width="20%" center="true">
      <el-form :model="loginForm" :label-width="formLabelWidth" :rules="loginRules" ref="loginForm">
        <el-form-item label="手机号" prop="phone">
          <el-input v-model="loginForm.phone" clearable></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="loginForm.password" show-password clearable></el-input>
        </el-form-item>
      </el-form>
      <div style=" color: gray; margin-top: 1rem;  font-size: 10px" align="center">
        还没账号？
        <span class="link" @click="handleRegister">点击注册</span>
      </div>
      <div slot="footer" class="dialog-footer">

        <el-button @click="loginFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="login">确 定</el-button>
      </div>
    </el-dialog>
    <!-- 注册对话框 -->
    <el-dialog title="注册" :visible.sync="registerFormVisible" width="20%" center="true">
      <el-form :model="form" :label-width="formLabelWidth" :rules="rules" ref="form">
        <el-form-item label="手机号" prop="phone">
          <el-input v-model="form.phone" clearable></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="form.password" show-password clearable></el-input>
        </el-form-item>
        <el-form-item label="重复密码" prop="password2">
          <el-input v-model="form.password2" show-password clearable></el-input>
        </el-form-item>
        <el-form-item label="姓名" prop="name">
          <el-input v-model="form.name" clearable></el-input>
        </el-form-item>
        <el-form-item label="省">
          <el-input v-model="form.province" clearable></el-input>
        </el-form-item>
        <el-form-item label="市">
          <el-input v-model="form.city" clearable></el-input>
        </el-form-item>
        <el-form-item label="区">
          <el-input v-model="form.district" clearable></el-input>
        </el-form-item>
        <el-form-item label="详细地址">
          <el-input v-model="form.address" clearable></el-input>
        </el-form-item>
      </el-form>
      <div style="color: gray; margin-top: 1rem;  font-size: 10px" align="center">
        已有账号？
        <span class="link" @click="handleLogin">直接登录</span>
      </div>
      <div slot="footer" class="dialog-footer">

        <el-button @click="registerFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="sureToRegister">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  data() {
    var validatePass2 = (rule, value, callback) => {
      if (value !== this.form.password) {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };
    return {
      activeIndex: "1",
      loginFormVisible: false,
      registerFormVisible: false,
      currentStatus: 0,
      status: {
        login: 0,
        register: 1,
        tourist: 2
      },
      loginForm: {
        phone: "",
        password: ""
      },
      form: {
        phone: "",
        password: "",
        password2: "",
        name: "",
        province: "",
        city: "",
        district: "",
        address: ""
      },
      formLabelWidth: "5rem",
      loginRules: {
        phone: [
          {
            required: true,
            message: "请输入手机号",
            trigger: ["blur", "change"]
          },
          {
            message: "请输入正确的手机号",
            trigger: ["blur", "change"]
          }
        ],
        password: [
          {
            required: true,
            message: "请输入密码",
            trigger: ["blur", "change"]
          }
        ]
      },
      rules: {
        phone: [
          {
            required: true,
            message: "请输入手机号",
            trigger: ["blur", "change"]
          },
          {
            message: "请输入正确的手机号",
            trigger: ["blur", "change"]
          }
        ],
        password: [
          { required: true, message: "请输入密码", trigger: ["blur", "change"] }
        ],
        password2: [
          {
            required: true,
            message: "请再次输入密码",
            trigger: ["blur", "change"]
          },
          { validator: validatePass2, trigger: "blur" }
        ],
        name: [
          {
            required: true,
            message: "请输入姓名",
            trigger: ["blur", "change"]
          }
        ]
      }
    };
  },
  computed: {
    isLogin() {
      return sessionStorage.getItem("uid") != null;
    }
  },

  
  methods: {
    handleSelect(key, keyPath) {
      this.activeIndex = key;
      switch (key) {
        case "1":
          this.$router.push({ name: "home" });
          break;
        case "2":
          this.$router.push({ name: "cart" });
          break;
        case "3":
          this.$router.push({ name: "order" });
          break;
        case "4":
          this.$router.push({ name: "my" });
          break;
      }
    },
    handleRegister() {
      this.loginFormVisible = false;
      this.registerFormVisible = true;
    },
    handleLogin() {
      this.registerFormVisible = false;
      this.loginFormVisible = true;
    },
    handleLogout() {
      sessionStorage.removeItem("uid");
      this.$router.go(0);
    },
    sureToRegister() {
      var user = {
        Phone: this.form.phone,
        Password: this.form.password,
        Name: this.form.name,
        Province: this.form.province,
        City: this.form.city,
        District: this.form.district,
        Address: this.form.address
      };
      this.$axios
        .post(this.serverUrl + "/api/user", user)
        .then(response => {
          console.log(response);
          sessionStorage.setItem("uid", response.data.Id);
          sessionStorage.setItem("phone", response.data.Phone);
          this.registerFormVisible = false;
          this.$router.go(0);
        })
        .catch(error => {
          this.$message.error(error.request.response);
        });
    },
    login() {
      if (this.loginForm.phone.length == 11) {
        this.$axios
          .post(this.serverUrl + "/api/user/login", {
            Id: this.loginForm.phone,
            Password: this.loginForm.password
          })
          .then(response => {
            sessionStorage.setItem("uid", response.data.Id);
            sessionStorage.setItem("phone", response.data.Phone);
            this.loginFormVisible = false;
            this.$router.go(0);
          })
          .catch(error => {
            this.$message.error(response.data.Message);
          });
      } else {
        this.$axios
          .post(this.serverUrl + "/api/admin/login", {
            Id: this.loginForm.phone,
            Password: this.loginForm.password
          })
          .then(response => {
            sessionStorage.setItem("uid", response.data.Id);
            sessionStorage.setItem("admin", response.data.Id);
            this.loginFormVisible = false;
            this.$router.push({ name: "admin" });
          })
          .catch(error => {
            this.$message.error(response.data.Message);
          });
      }
    }
  }
};

window.addEventListener("scroll", function () {
    let header = document.querySelector("header");
    header.classList.toggle("sticky", window.scrollY > 0);
})

</script>


<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #404040;
}
.el-input_inner{
  width: 40px;
}
#nav {
  padding: 30px;
}

.logo{
  font-weight: bolder;
  font-size: 40px;
}
#nav a {
  font-weight: bold;
  color: #404040;
}

a {
  text-decoration: none;
}

.horizontal {
  display: flex;
  flex-wrap: wrap;
}

.link:hover {
  color: #409eff;
  cursor: pointer;
}
header
{
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-weight: bolder;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  transition:0.6s;
  padding: 40px 100px;
  z-index: 100000;
  box-shadow: aliceblue;
  border-bottom: darkgrey;
}

header.sticky
{
  padding: 5px 100px;
  background: white;
  box-shadow:0px 12px 8px -10px #000;
}
@keyframes bganimation {
  0%{
    background-position: 0% 50%;
  }
  50%{
    background-position: 100% 50%;
  }
  100%{
    background-position: 0% 50%;
  }

}
header.logo
{
  position: relative;
  font-weight: 700;
  color: white;
  text-decoration: none;
  font-size: 2em;
  text-transform: uppercase;
  Letter-spacing: 2px;
  tansition:0.6s;
}

header ul
{
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  font-style: italic;
}
header ul li
{
  position: relative;
  list-style: none;
}

header ul li a
{
  position: relative;
  margin: 0 15px;
  text-decoration: none;
  color: white;
  letter-spacing: 2px;
  font-weight: 500;
  transition: 0.6s;
  font-size: 20px;
}

header.sticky .logo,
header.sticky ul li  a
{
  color: black;

}
.banner
{
  position: relative;
  width: 100%;
  height:50vh;
  /*background: url("../img/background.jpg");*/
  background-size: cover;
}

body
{
  background-image: linear-gradient(125deg,#9b7ddb, #dc9996, #dbc2c9);
  min-height: 200vh;
  background-size: 400%;
  animation: bganimation 15s infinite;
}
</style>
