<template>
  <div style="
        display: flex;
        justify-content: right;
        align-items: right;
        height: 50px;
        float: right;
      ">
    <div
      style="margin: auto"
    >
      <button v-if="this.$route.params.id!=undefined" class="btn btn-dark" style="height: auto" @click="beforeDirectory()">Back</button>
    </div>
  </div>
</template>

<script>
export default {
  methods: {
    beforeDirectory() {
      if (this.$store.state.directory.length > 1) {
        // console.log(this.$store.state.directory);
        this.poplog = this.$store.state.directory.pop();
      // console.log(this.$store.state.directory);
      // console.log(this.poplog);


      this.$store.state.path = "";
      this.pathDirectory = "/Share"+decodeURIComponent(this.$route.params.id);
      // console.log(cnt)
      // console.log(this.$store.state.directory);
      // for (var i = 0; i < this.$store.state.directory.length; i++) {
      //   this.pathDirectory += this.$store.state.directory[i].replace(
      //     "Home",
      //     ""
      //   );
      //   this.$store.state.path += this.$store.state.directory[i].replace(
      //     "Home",
      //     ""
      //   );
      //   // console.log(item)
      // }
      // console.log(this.pathDirectory);
      // this.$store.state.path = this.pathDirectory;
      if (this.$store.state.directory.length == 1){
        this.$router.push("/Share/");
      }else{
      this.$router.push(
        "/Share/" +
          encodeURIComponent(decodeURIComponent(this.$route.params.id).replace(this.poplog, ""))
      );
      }
      console.log("eiei");
      }
      this.getData();
      console.log(this.$store.state.directory.length);
    },
    getAllData(){
      this.GetAllData.path = "/uploads/"
      console.log(this.GetData);
      this.$axios
        .post("DataFile/GetAllDataFiles", this.GetAllData, {
          headers: {
            Authorization:
              "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.$store.state.allDataFile = response.data;
        })
        .catch((error) => {
          console.error(error);
          console.log("error");
        });
    },
    getData() {
      //   console.log(this.pathDirectory);
      // this.GetData.iduser = localStorage.IdUser;
      this.GetData.path = "/uploads";
      if (this.$route.params.id == undefined) {
        this.GetData.path = null;
      } else {
        // console.log(this.pathDirectory);
        this.GetData.path += decodeURIComponent(this.$route.params.id);
        // console.log(this.pathFile.path);
      }
      console.log(this.GetData.path)
      this.$axios
        .post("DataFileShare/getdata", this.GetData, {
          headers: {
            Authorization:
              "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.$store.state.dataFile = response.data;
          this.$store.state.defaultDataFile =response.data;
          this.getAllData();
          //   console.log(this.dataFile);
        })
        .catch((error) => {
          console.error(error);
        });
    },
  },
  data() {
    return {
      poplog:undefined,
      GetAllData:{
        path: undefined,
      },
       GetData:{
        path: "",
        iduser: 24,
      },
      pathDirectory: "",
      pathFile: {
        path: "/uploads",
      },
      dataFile: [
        {
          iD: undefined,
          nameFile: "",
          path: "",
          type: "",
          file: "",
          filedata: "",
        },
      ],
    };
  },
};
</script>

<style>
</style>