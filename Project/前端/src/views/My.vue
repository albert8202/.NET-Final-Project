<template>
  <div class="my" style="margin-top: 280px">
    <h3 v-if="!isLogin" style="text-align: center;">请先登录</h3>
    <div v-else>
      <div id="info">
        <h3>个人信息</h3>
        <div class="info-item">
          <span class="title">姓名</span>
          <span>{{newUser.Name}}</span>
        </div>
        <div class="info-item">
          <span class="title">手机</span>
          <span>{{newUser.Phone}}</span>
        </div>
        <div class="info-item">
          <span class="title">省</span>
          <el-input v-model="newUser.Province" style="width: 15rem;"></el-input>
        </div>
        <div class="info-item">
          <span class="title">市</span>
          <el-input v-model="newUser.City" style="width: 15rem;"></el-input>
        </div>
        <div class="info-item">
          <span class="title">区</span>
          <el-input v-model="newUser.District" style="width: 15rem;"></el-input>
        </div>
        <div class="info-item">
          <span class="title">地址</span>
          <el-input v-model="newUser.Address" style="width: 15rem;"></el-input>
        </div>
      </div>
      <el-button plain style="margin-left: 52rem" @click="reset">重 置</el-button>
      <el-button type="primary" plain @click="modify">确认修改</el-button>
    </div>
  </div>
</template>

<script>
export default {
  name: "my",
  data() {
    return {
      user: {},
      newUser: {}
    };
  },
  computed: {
    isLogin() {
      return sessionStorage.getItem("uid") != null;
    }
  },
  mounted() {
    if (!this.isLogin) {
      return;
    }
    this.$axios
      .get(this.serverUrl + "/api/user/" + sessionStorage.getItem("uid"))
      .then(response => {
        this.user = response.data;
        this.newUser = JSON.parse(JSON.stringify(response.data));
      })
      .catch(error => {
        this.$message.error("出错啦");
      });
  },
  methods: {
    reset() {
      this.newUser = this.user;
    },
    modify() {
      this.$axios
        .put(this.serverUrl + "/api/user", this.newUser)
        .then(response => {
          this.user = JSON.parse(JSON.stringify(this.newUser));
          this.$message.success("修改成功");
        })
        .catch(error => {
          this.$message.error("出错啦");
        });
    }
  }
};

</script>

<style scoped>
h3 {
  text-align: center;
}

#info {
  margin: 2rem 20rem;
  font-size: 1.25rem;
  background-color: whitesmoke;
  padding: 2rem;
  border-radius: 1.25rem;
}

.info-item {
  margin: 4rem;
}

.title {
  font-weight: bold;
  margin: 0 6rem;
}
</style>
