<template>
  <div>
    <img
      style="display: block; margin: auto;"
      class=""
      v-if="path != null"
      :src="path"
      alt="Card image cap"
    />
    <p v-if="path == null">ไม่มีสิทธิ์เข้าถึงภาพ</p>
    <!-- <button v-on:click="Test()">asdasd</button> -->
  </div>
</template>

<script>
export default {
  methods: {
    Photo() {
      this.$axios
        .post("Photo/GetPhoto", this.GetData, {
          headers: {
            Authorization: "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.path = response.data.wwwPath;
          console.log(response);
        })
        .catch((error) => {
          console.error(error);
        });
    },
  },
  mounted() {
    console.log(this.GetData.path);
    this.Photo();
  },

  data() {
    return {
      GetData: {
        path: decodeURIComponent(this.$route.params.path),
      },
      path: null,
    };
  },
};
</script>

<style>
</style>