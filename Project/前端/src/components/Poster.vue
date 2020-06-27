<template>
  <div class="poster">
    <el-card :body-style="{ padding: '0px' }">
      <img :src="imgUrl" class="image">
    </el-card>
  </div>
</template>

<script>
export default {
  name: "poster",
  props: {
    imgUrl: String
  }
};


</script> 

<style>
</style>


