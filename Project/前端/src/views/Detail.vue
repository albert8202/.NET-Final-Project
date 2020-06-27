<template>
  <div class="detail" style="margin-top: 300px">
    <el-button type="primary" icon="el-icon-back" circle @click="handleBack"></el-button>
    <div class="horizontal">
      <img :src="this.album.CoverUrl" style="margin: 0 50px; width: 20rem; height:30rem">
      <div>
        <h2 style="text-align: left">{{album.Name}}</h2>
        <div >
          <info-item :title="'表演者'" :item=album.Author ></info-item>
          <info-item :title="'出版者'" :item=album.Publisher></info-item>
          <info-item :title="'流派'" :item=album.Category></info-item>
          <info-item :title="'ISRC'" :item=album.Isbn></info-item>
          <info-item :title="'价格'" :item=album.Price></info-item>
          <info-item :title="'销量'" :item=album.Sales></info-item>
          <info-item :title="'库存'" :item=album.Stock></info-item>
        </div>
        <div style="width: 500px">
        <div class="summary">
          <h3 align="left">简介</h3>
          <div>{{album.Description}}</div>
        </div>
        </div>
        <el-input-number v-model="num" @change="handleChange" :min="num" :max="album.Stock" label="数量"></el-input-number>
        <el-button type="primary" plain @click="handleAddCart" style="margin-left: 30px">加入购物车</el-button>
      </div>
    </div>
  </div>
</template>

<script>
import InfoItem from "../components/InfoItem";

export default {
  name: "detail",
  components: {
    InfoItem
  },
  data() {
    return {
      album: {
          Author:"",
          Publisher:"",
          Category:"",
          Isbn:0,
          Price:0,
          Sales:0,
          Stock:0,
          Id:0


      },
      num: 1
    };
  },
  
  mounted() {
      console.log(this.$route.params.id);
    this.$axios
      .get(this.serverUrl + "/api/Album/" + `${this.$route.params.id}`)
      .then(response => {
          console.log(response.data);
        this.album = response.data;
        console.log(this.album)
      })
      .catch(error => {
        this.$message.error(error);
      });
  },
  methods: {
    handleBack() {
      this.$router.push({ name: "home" });
    },
    handleChange(value) {},
    handleAddCart() {
      if (sessionStorage.getItem("uid") == null) {
        this.$message.error("请先登录");
        return;
      }
      var request = {
        UserId: sessionStorage.getItem("uid"),
        AlbumId: this.album.Id,
        Count: this.num
      };
      this.$axios
        .post(this.serverUrl + "/api/cart", request)
        .then(response => {
            console.log(response);
          this.$message.success("添加成功");
        })
        .catch(error => {
          this.$message.error(error);
        });
    }
  }
};
</script>

<style scoped>
.detail {
  padding: 0 0 0 100px;
}

li {
  list-style: none;
}

.summary h3 {
  text-align: center;
}

.summary {

  width: 80%;
  background-color: whitesmoke;

  font-size: 15px;
  opacity:0.6;
  margin-top:30px;
  padding-top: 10px;
  padding-bottom: 8px;
  margin-bottom: 50px;
}
</style>
