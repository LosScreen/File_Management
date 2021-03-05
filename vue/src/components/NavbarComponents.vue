<template>
  <div
    class="row w-100"
    style="background-color: white;text-align:left;border-bottom:1px gray solid;margin:0;padding: 10px; 0px"
  >
    <div class="col-10">
      <i class="fab fa-google-drive" style="font-size: 25px; margin-top: 10px">
        <div style="display: inline-block; width: 200px; margin: 0 0 0 10px">
          Piyapon Drive
        </div>
      </i>
      <div style="" class="input-box">
        <i class="fas fa-search" style="margin: 0 10px 0 0"></i>
        <!-- @keyup.enter="asd" v-on:keydown="Search" -->
        <form style="display: inline-block;">
        <input
          
          placeholder="ค้นหาที่ใช้ไม่ได้จริง"
          type="text"
          style=""
          class="input-search"
          v-model="searchText"
          v-on:keyup="Search"
        />
        </form>
      </div>
    </div>
    <div class="col" style="text-align: right; margin: 12px 0 0 0">
      <div class="Username" style="display: inline">{{ this.UserName }}</div>
      <div class="logout_btm" style="display: inline" @click="LogOut()">
        <!-- {{localStorage.Username}}  -->
        logout
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Menu",
  data() {
    return {
      UserName: localStorage.Username,
      searchText: "",
      dataFile: [
        {
          iD: null,
          nameFile: "",
          path: "",
          type: "",
          file: "",
          filedata: "",
        },
      ],
    };
  },
  methods: {
    LogOut() {
      localStorage.Username = "";
      localStorage.Token = "";
      this.$router.push("/Login");
    },
    Search() {
      // console.log(
      //   this.$store.state.dataFile.filter(
      //     (item) => item.nameFile == this.searchText
      //   )
      // );
      console.log(this.searchText);
      // this.$store.state.dataFile.filter(item => item.nameFile == this.searchText)
    },
  },
  // watch: {
  //     searchText:undefined,
  //   },
  // computed: {
  //   asd() {
  //     return alert(this.searchText);
  //   },
  // },
  mounted() {
    // console.log(this.UserName);
  },
};
</script>

<style>
.logout_btm {
  color: rgb(71, 132, 245);
  cursor: pointer;
}
.logout_btm:hover {
  color: rgb(47, 87, 163);
}
.Username {
  font-weight: bold;
}
.input-box {
  display: inline-block;
  margin-left: 40px;
  background-color: rgb(228, 228, 228);
  border-radius: 10px;
  height: 60px;
  width: 470px;
  padding: 10px 0 0 25px;
}
.input-search {
  border-radius: 10px;
  height: 40px;
  width: 400px;
  border: none;
  background-color: transparent;
}
.user-box {
  height: 50px;
  width: 50px;
  border-radius: 25px;
  background-color: green;
  padding: 13px;
  margin: 0px 20px 0 0;
  text-align: center;
}
</style>