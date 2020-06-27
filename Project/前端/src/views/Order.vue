<template>
  <div class="order" style="margin-top: 250px">
    <h3 v-if="!isLogin" style="text-align: center;">您还未登陆哦</h3>
    <div v-else>
      <el-tabs tab-position="left" style="height: auto;">
        <el-tab-pane label="待支付">
          <div class="block">
            <el-timeline :reverse="true" style="margin-right: 50px">
              <el-timeline-item
                v-for="order in notPaid"
                :key="order.Id"
                :timestamp="order.OrderTime"
                placement="top"
                :hide-timestamp="hideTimestamp"
              >
                <el-card>
                  <p>订单号：{{order.Id}}</p>
                  <h4 v-for="c1 in order.Carts" :key="c1.Id">《{{c1.Album.Name}}》 * {{c1.Count}} 张</h4>
                  <span>收件人：{{order.ReceiverName}}</span>
                  <span>联系方式：{{order.ReceiverPhone}}</span>
                  <span>地址：{{order.ReceiverAddress}}</span>
                  <p>
                    <span>运费：{{order.DeliveryFee}}</span>
                    <span>总计：{{order.TotalPrice}}</span>
                    <el-button type="warning" size="mini" plain @click="payOrder(order.Id)">去支付</el-button>
                  </p>
                </el-card>
              </el-timeline-item>
            </el-timeline>
          </div>
        </el-tab-pane>
        <el-tab-pane label="未送达">
          <div class="block">
            <el-timeline :reverse="true">
              <el-timeline-item
                v-for="order in notReceive"
                :key="order.Id"
                :timestamp="order.OrderTime"
                placement="top"
                :hide-timestamp="hideTimestamp"
              >
                <el-card>
                  <p>订单号：{{order.Id}}</p>
                  <h4 v-for="c2 in order.Carts" :key="c2.Id">《{{c2.Album.Name}}》 * {{c2.Count}} 张</h4>
                  <span>收件人：{{order.ReceiverName}}</span>
                  <span>联系方式：{{order.ReceiverPhone}}</span>
                  <span>地址：{{order.ReceiverAddress}}</span>
                  <p>
                    <span>运费：{{order.DeliveryFee}}</span>
                    <span>总计：{{order.TotalPrice}}</span>
                    <el-button type="warning" size="mini" plain @click="receiveOrder(order.Id)">确认收货</el-button>
                  </p>
                </el-card>
              </el-timeline-item>
            </el-timeline>
          </div>
        </el-tab-pane>
        <el-tab-pane label="已完成">
          <div class="block">
            <el-timeline :reverse="true">
              <el-timeline-item
                v-for="order in finish"
                :key="order.Id"
                :timestamp="order.OrderTime"
                placement="top"
                :hide-timestamp="hideTimestamp"
              >
                <el-card>
                  <p>订单号：{{order.Id}}</p>
                  <h4 v-for="c3 in order.Carts" :key="c3.Id">《{{c3.Album.Name}}》 * {{c3.Count}} 张</h4>
                  <span>收件人：{{order.ReceiverName}}</span>
                  <span>联系方式：{{order.ReceiverPhone}}</span>
                  <span>地址：{{order.ReceiverAddress}}</span>
                  <p>
                    <span>运费：{{order.DeliveryFee}}</span>
                    <span>总计：{{order.TotalPrice}}</span>
                  </p>
                </el-card>
              </el-timeline-item>
            </el-timeline>
          </div>
        </el-tab-pane>
        <el-tab-pane label="全部订单">
          <div class="block">
            <el-timeline :reverse="true">
              <el-timeline-item
                v-for="order in orders"
                :key="order.Id"
                :timestamp="order.OrderTime"
                placement="top"
                :hide-timestamp="hideTimestamp"
              >
                <el-card>
                  <p>订单号：{{order.Id}}</p>
                  <h4 v-for="c4 in order.Carts" :key="c4.Id">《{{c4.Album.Name}}》 * {{c4.Count}} 张</h4>
                  <span>收件人：{{order.ReceiverName}}</span>
                  <span>联系方式：{{order.ReceiverPhone}}</span>
                  <span>地址：{{order.ReceiverAddress}}</span>
                  <p>
                    <span>运费：{{order.DeliveryFee}}</span>
                    <span>总计：{{order.TotalPrice}}</span>
                  </p>
                </el-card>
              </el-timeline-item>
            </el-timeline>
          </div>
        </el-tab-pane>
      </el-tabs>
    </div>
  </div>
</template>

<script>
export default {
  name: "order",
  data() {
    return {
      orders: [],
      notPaid: [],
      notReceive: [],
      finish: [],
      hideTimestamp: false
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
      .get(this.serverUrl + "/api/order", {
        params: {
          userId: sessionStorage.getItem("uid")
        }
      })
      .then(response => {
        this.orders = response.data;
        for (var i in this.orders) {
          switch (this.orders[i].Status) {
            case 0:
              this.notPaid.push(this.orders[i]);
              break;
            case 1:
              this.notReceive.push(this.orders[i]);
              break;
            case 2:
              this.finish.push(this.orders[i]);
              break;
          }
        }
        for (var i in this.orders) {
          this.getCart(i, this.orders[i].Id);
        }
      })
      .catch(error => {
        this.$message.error("出错啦");
      });
  },
  methods: {
    getCart(index, orderid) {
      var carts = [];
      this.$axios
        .get(this.serverUrl + "/api/order/" + orderid)
        .then(response => {
          carts = response.data;
          this.orders[index]["Carts"] = carts;
          for (var i in carts) {
            this.getAlbum(i, carts[i].AlbumId, index);
          }
        })
        .catch(error => {
          this.$message.error(error);
          console.log("orderID",orderid)
            console.log("cart",this.orders)
        });
    },
    getAlbum(index, albumid, orderindex) {
      this.$axios
        .get(this.serverUrl + "/api/album/" + albumid)
        .then(response => {
          this.orders[orderindex].Carts[index].Album = response.data;
        })
        .catch(error => {
          this.$message.error(error);
        });
    },
    payOrder(orderId) {
      this.$confirm("是否确认支付？", "支付", {
        confirmButtonText: "确定",
        cancelButtonText: "取消"
      })
        .then(() => {
          this.$axios
            .put(this.serverUrl + "/api/order/" + orderId)
            .then(() => {
              this.$message.success("支付成功");
              this.$router.go(0);
            })
            .catch(() => {
              this.$message.error("支付失败");
                console.log("cart",this.carts)
                console.log("orders",this.orders)
            });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "请稍后支付"
          });
        });
    },
    receiveOrder(orderId) {
      this.$axios
        .put(this.serverUrl + "/api/order/" + orderId)
        .then(() => {
          this.$message.success("已确认");
          this.$router.go(0);
        })
        .catch(() => {
          this.$message.error("确认失败");
        });
    }
  }
};
</script>

<style scoped>
.el-card span {
  margin: 0 4rem 0 10rem;
}

</style>
