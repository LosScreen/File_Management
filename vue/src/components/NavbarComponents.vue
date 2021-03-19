<template>
  <div
    class="row w-100"
    style="background-color: white;text-align:left;border-bottom:1px gray solid;margin:0;padding: 10px; 0px"
  >
    <div class="col-10">
      <i class="fab fa-google-drive logo" style="font-size: 25px; margin-top: 10px" @click="Home()">
        <div class="logo" style="display: inline-block; width: 200px; margin: 0 0 0 10px" @click="Home()">
          Piyapon Drive
        </div>
      </i>
      <div style="" class="input-box">
        <i class="fas fa-search" style="margin: 0 10px 0 0"></i>
        <!-- @keyup.enter="asd" v-on:keydown="Search" -->
        <form style="display: inline-block;">
          <input
            placeholder="ค้นหาที่ใช้ได้จริง"
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
      GetAllData: {
        path: undefined,
      },
      GetData: {
        path: undefined,
      },
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
    getAllData() {
      this.GetAllData.path = "/uploads/";

      this.$axios
        .post("DataFile/GetAllDataFiles", this.GetAllData, {
          headers: {
            Authorization: "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.$store.state.allDataFile = response.data;
        })
        .catch((error) => {
          console.error(error);
          // console.log("error");
        });
    },
    getData() {
      this.GetData.path = "/uploads";

      if (this.$route.params.id == undefined) {
        this.GetData.path = "/uploads";
      } else {
        this.GetData.path += decodeURIComponent(this.$route.params.id);
      }

      if (this.$route.params.id != "/" + localStorage.Username) {
        var str = this.$route.params.id.split("/");
        var mapstr = str.map((str) => "/" + str);

        this.$store.state.directory = ["Home"];
        for (var i = 0; i < mapstr.length - 2; i++) {
          this.$store.state.directory.push(mapstr[i + 2]);
          this.$store.state.path += mapstr[i + 1];
        }
      }

      this.$axios
        .post("DataFile/getdata", this.GetData, {
          headers: {
            Authorization: "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.$store.state.dataFile = response.data;
          this.$store.state.defaultDataFile = response.data;
          this.getAllData();
        })
        .catch((error) => {
          console.error(error);
          // console.log("error");
        });
    },
    Home() {
      this.$store.state.directory = ["Home"];
      this.$router.push(
        "/MyDrive/" + encodeURIComponent("/" + localStorage.Username)
      );
      this.getData();
    },
    LogOut() {
      localStorage.Username = "";
      localStorage.Token = "";
      this.$router.push("/Login");
    },
    Search() {
      // var a = {i:0,b:1}
      if (this.searchText != "") {
        this.$store.state.dataFile = [];
        // || data.nameFile.indexOf(this.searchText) != -1 || data.nameFile.indexOf(this.searchText.toUpperCase()) != -1
        this.$store.state.allDataFile.forEach((data) => {
          if (data.nameFile.toLowerCase().indexOf(this.searchText.toLowerCase()) != -1) {
            // console.log("เจออยู่จ้า");
            this.$store.state.dataFile.push(data);
            // console.log(this.$store.state.dataFile);
          }
          // console.log(this.$store.state.allDataFile);
        });
      } else if (this.searchText == "") {
        // console.log("เข้ามาแล้ว");
        this.$store.state.dataFile = this.$store.state.defaultDataFile;
      }
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
.logo {
  cursor: pointer;
}
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
