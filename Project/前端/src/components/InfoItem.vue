<template>
  <div class="info-item">
    <div class="item-title">
      <strong>{{ title }}</strong>
    </div>
    <div class="item-content">
      <p>{{ item }}</p>
    </div>
  </div>
</template>



<script>
export default {
  name: "info-item",
  props: {
    title: String,
    item: Object
  }
};
</script>

<style>
.info-item {
  display: flex;
  margin: 5px;
}

li {
  list-style: none;
}

.item-content p {
  text-align: left;
  margin: 0 20px;
}
</style>
