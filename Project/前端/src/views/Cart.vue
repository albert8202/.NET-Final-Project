<template>
  <div id="cart" style="margin-top: 280px">
    <h3 v-if="!isLogin" style="text-align: center;margin-top: 200px">您还未登录哦</h3>
    <div v-else style="margin-top: 20px">
      <el-checkbox-group v-model="cartList" style="margin-top: 20px">
        <el-checkbox v-for="cart in carts" :key="cart.Id" class="cart" :label="cart.Id">
          <div style="display: -webkit-flex;display: flex;-webkit-justify-content: space-around;justify-content: space-around;">
            <span style="width: 300px">{{cart.Album.Name}}</span>
            <span style="width: 300px">单价：{{cart.Album.Price}}</span>
            <span style="width: 30px">数量：{{cart.Count}}</span>
            <el-button type="danger" size="mini" plain @click="deleteCart(cart.Id)" style="margin-left: 600px">删除</el-button>
          </div>


        </el-checkbox>
      </el-checkbox-group>
      <el-button
        type="primary"
        plain
        style="float: right; margin-right: 2rem;"
        @click="beforeCreate"
      >生成订单</el-button>
    </div>
    <el-dialog title="订单信息" :visible.sync="centerDialogVisible" width="40%" center>
      <el-form :model="form" ref="form" label-width="100px">
        <el-form-item
          label="收件人"
          :rules="[
      { required: true, message: '请输入收件人姓名', trigger: ['blur', 'change'] }
    ]"
        >
          <el-input v-model="form.receiverName"></el-input>
        </el-form-item>
        <el-form-item
          label="联系方式"
          :rules="[
      { required: true, message: '请输入联系方式', trigger: ['blur', 'change'] }
    ]"
        >
          <el-input v-model="form.receiverPhone"></el-input>
        </el-form-item>
        收件地址
        <el-form-item
          label="省"
          :rules="[
      { required: true, message: '请输入地址', trigger: ['blur', 'change'] }
    ]"
        >
          <el-input v-model="form.receiverAddress[0]"></el-input>
        </el-form-item>
        <el-form-item
          label="市"
          :rules="[
      { required: true, message: '请输入地址', trigger: ['blur', 'change'] }
    ]"
        >
          <el-input v-model="form.receiverAddress[1]"></el-input>
        </el-form-item>
        <el-form-item
          label="区"
          :rules="[
      { required: true, message: '请输入地址', trigger: ['blur', 'change'] }
    ]"
        >
          <el-input v-model="form.receiverAddress[2]"></el-input>
        </el-form-item>
        <el-form-item
          label="详细地址"
          :rules="[
      { required: true, message: '请输入地址', trigger: ['blur', 'change'] }
    ]"
        >
          <el-input v-model="form.receiverAddress[3]"></el-input>
        </el-form-item>
      </el-form>

      <span slot="footer" class="dialog-footer">
        <el-button @click="centerDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="createOrder">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
export default {
  name: "cart",
  data() {
    return {
      user: {},
      carts: [],
      cartList: [],
      centerDialogVisible: false,
      form: {
        userId: sessionStorage.getItem("uid"),
        cartList: [],
        receiverName: "",
        receiverPhone: "",
        receiverAddress: ["", "", "", ""]
      }
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
    this.$axios.get(this.serverUrl + "/api/cart", {
        params: {
          userId: sessionStorage.getItem("uid")
        }
      })
      .then(response => {
          console.log(response.data,"hahaha");
        this.carts = response.data;
        for (var i in this.carts) {
          this.getAlbum(i, this.carts[i].AlbumId);
          console.log("weiwei",this.carts)
        }
      })
      .catch(error => {
        this.$message.error("出错啦");
      });

    this.$axios
      .get(this.serverUrl + "/api/user", {
        params: {
          id: sessionStorage.getItem("uid")
        }
      })
      .then(response => {
        this.user = response.data;
        console.log("user",this.user)
      })
      .catch(error => {
        this.$message.error("出错啦");
      });
  },
  methods: {
    getAlbum(_index, _albumId) {
      this.$axios
        .get(this.serverUrl + "/api/album/" + _albumId)
        .then(response => {
          this.carts[_index].Album = response.data;
        })
        .catch(error => {
          this.$message.error(error);
        });
    },
    beforeCreate() {
      if (this.cartList.length <= 0) {
        this.$message.warning("没有选中商品");
        return;
      }
      this.form.receiverName = this.user.Name;
      this.form.receiverPhone = this.user.Phone;
      this.form.cartList = this.cartList;
      this.form.receiverAddress[0] = this.user.Province;
      this.form.receiverAddress[1] = this.user.City;
      this.form.receiverAddress[2] = this.user.District;
      this.form.receiverAddress[3] = this.user.Address;
      this.centerDialogVisible = true;
      console.log("form",this.form)
    },
    createOrder() {
      this.centerDialogVisible = false;
      var orderid = 0;
      this.$axios
        .post(this.serverUrl + "/api/order", this.form)
        .then(response => {
          this.$message.success("下单成功");
          orderid = response.data;
          this.$confirm("是否确认支付？", "支付", {
            confirmButtonText: "确定",
            cancelButtonText: "取消"
          })
            .then(() => {
              this.$axios
                .put(this.serverUrl + "/api/order/" + orderid)
                .then(() => {
                  this.$message.success("支付成功");
                  this.$router.go(0);
                })
                .catch(() => {
                  this.$message.error("支付失败");
                  this.$router.go(0);
                });
            })
            .catch(() => {
              this.$message({
                type: "info",
                message: "请稍后在订单页面进行支付"
              });
              this.$router.go(0);
            });
        })
        .catch(error => {
          this.$message.error(error);
        });
    },
    deleteCart(cartid) {
      this.$axios
        .delete(this.serverUrl + "/api/cart/" + cartid)
        .then(() => {
          this.$message.success("删除成功");
          this.$router.go(0);
        })
        .catch(error => {
          this.$message.error("删除失败");
        });
    }
  }
};
</script>

<style scoped>
#cart {
  margin: 0 10rem;
}

.cart {
  display: block;
  margin: 0.5rem 1rem;
  padding: 0.75rem;
  background-color: whitesmoke;
  border-radius: 0.5rem;
}

.cart span {
  margin: 0 2rem;
}
</style>
