<template>
  <div class="home" style="margin-top: 200px">
    <div class="table-wrapper" style="width: 1600px; margin-left:300px">
    <el-table :data="pageData" stripe style="width: 80%" align="center">
      <el-table-column prop="CoverUrl" label="封面" width="250%">
        <template slot-scope="scope">
          <img :src="scope.row.CoverUrl" height="150px">
        </template>
      </el-table-column>
      <el-table-column prop="Name" label="唱片名" width="300%" style="margin-left: 10px"></el-table-column>
      <el-table-column prop="Author" label="表演者" width="150%"></el-table-column>
      <el-table-column prop="Price" label="价格" width="150%"></el-table-column>

      <el-table-column>
        <template slot="header" slot-scope="scope">
          <input style="font-size: 20px" v-model="search" placeholder="输入关键字搜索" @keyup.enter="handleSearch">
          <el-button icon="el-icon-search" circle size="mini" @click="handleSearch"></el-button>
        </template>
        <template slot-scope="scope">
          <el-button size="mini" @click="handleLookdetail(scope.$index, scope.row)">查看详情</el-button>
        </template>
      </el-table-column>
    </el-table>
    </div>
    <div class="pagination" style="margin-top: 10px;margin-right: 300px">
      <el-pagination
        layout="total, prev, pager, next, jumper"
        :total="total"
        :page-size="10"
        :current-page="currentPage"
        @current-change="handlePageChange"
      ></el-pagination>
    </div>
    <div>
      <div class="advantage-list-container">
        <h2>线上精品店专享服务</h2>

        <ul >
          <li>
            <img src="img/deliver-icon.svg">
            <h3 >免费配送</h3>
            <div class="advantage-item-text"><p>所有订单均享免费顺丰速递</p></div>
          </li>
          <li>
            <img src="img/package-icon.svg">
            <h3>艺术包装</h3>
            <div class="advantage-item-text"><p>您的订单将尊享艺术包装并随附个性化定制祝福</p></div>
          </li>
          <li >
            <img src="img/refresh-icon.svg" >
            <h3 >7天无理由退货</h3>
            <div class="advantage-item-text"><p>非定制商品订单自签收日起7天内可享无理由退货</p></div>
          </li>
        </ul>
      </div>
    </div>
    <footer>
      <span>版权所有 同济大学软件学院 李佳诺</span>
    </footer>
  </div>
</template>

<script>
export default {
  name: "home",
  data() {
    return {
      books: [],
      searchData: [],
      search: "",
      currentPage: 1,
      fullData: true
    };
  },
  mounted() {
    this.$axios
      .get(this.serverUrl + "/api/album")
      .then(response => {
        this.books = response.data;
      })
      .catch(error => {
        this.$message.error(error);
      });
  },
  methods: {
    handleLookdetail(index, row) {
      this.$router.push({ name: "detail", params: { id: row.Id } });
    },
    handlePageChange(page) {
      this.currentPage = page;
    },
    handleSearch() {
      this.fullData = false;
      var result = [];
      var re = new RegExp(this.search, "i");
      for (var i = 0; i < this.books.length; i++) {
        if (JSON.stringify(this.books[i]).match(re)) {
          result.push(this.books[i]);
        }
      }
      this.currentPage = 1;
      this.searchData = result;
    }
    
  },
  computed: {
    pageData() {
      var pageData = [];
      var begin = (this.currentPage - 1) * 10;
      var end = this.currentPage * 10;
      if (this.fullData) {
        for (var i = begin; i < end && i < this.books.length; i++) {
          pageData.push(this.books[i]);
        }
      } else {
        for (var j = begin; j < end && j < this.searchData.length; j++) {
          pageData.push(this.searchData[j]);
        }
      }
      return pageData;
    },
    total() {
      if (this.fullData) {
        return this.books.length;
      } else {
        return this.searchData.length;
      }
    }
  }
};
</script>

<style>

  .table-wrapper /deep/ .el-table tr , .el-table th {
    background-color: transparent;
  }

  .table-wrapper /deep/.el-table thead {
    color: black;
    font-weight: 500;
    font-size: 20px;
    background-color: rgba(148, 144, 144, 0.3);
    line-height:45px
  }

  .el-table .cell{
    margin-left: 20px;
  }
input {
  border-radius: 25px;
  height: 20px;
  padding: 3px;
  border: none;
  outline: none;
  background-color: whitesmoke;
}
input:hover {
  background-color: #40a0ff13;
}

.pagination{
  float: right;
}

.advantage-list-container{
  margin-top: 80px;
  background-color: black;
  padding: 50px;
  text-align: center;
  color: white;
  height: 350px;
}
.advantage-list-container ul
{
  display: inline-flex;
  padding-top: 40px;
  justify-content: space-between;
}
.advantage-list-container li{
  width: 20%;
  align-items: center;

  list-style-type: none;
}
.advantage-list-container h2{
  font-size: 32px;
  font-weight: 400;
  font-family: "Century Gothic Std";
}

.advantage-list-container h3{
  margin-bottom: 20px;
}
footer{
  text-align: center;
  color: white;
  font-family: Times New Roman;
  font-weight: bold;
}
</style>
