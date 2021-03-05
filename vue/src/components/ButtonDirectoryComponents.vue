<template>
  <div style="
        display: flex;
        justify-content: right;
        align-items: right;
        height: 50px;
        float: right;
      ">
    <div
      style="margin: auto" v-if="this.$store.state.directory.length >1"
    >
      <button class="btn btn-dark" style="height: auto" @click="beforeDirectory()">Back</button>
    </div>
  </div>
</template>

<script>
export default {
  methods: {
    beforeDirectory() {
      if (this.$store.state.directory.length > 1) {
        // console.log(this.$store.state.directory);
        this.$store.state.directory.pop();
      }
      console.log(this.$store.state.directory);
      // console.log(j);
      this.$store.state.path = "";
      this.pathDirectory = "/uploads/"+localStorage.Username;
      // console.log(cnt)
      // console.log(this.$store.state.directory);
      for (var i = 0; i < this.$store.state.directory.length; i++) {
        this.pathDirectory += this.$store.state.directory[i].replace(
          "Home",
          ""
        );
        this.$store.state.path += this.$store.state.directory[i].replace(
          "Home",
          ""
        );
        // console.log(item)
      }
      // console.log(this.pathDirectory);
      // this.$store.state.path = this.pathDirectory;
      this.$router.push(
        "/MyDrive/" +
          encodeURIComponent(this.pathDirectory.replace("/uploads", ""))
      );
      this.getData();
      // console.log(this.pathDirectory);
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
      this.GetData.path = "/uploads/"+localStorage.Username;
      if (this.pathDirectory == "/uploads/"+localStorage.Username) {
        this.GetData.path = "/uploads/"+localStorage.Username;
      } else {
        // console.log(this.pathDirectory);
        this.GetData.path = this.pathDirectory;
        // console.log(this.pathFile.path);
      }
      console.log(this.GetData.path)
      this.$axios
        .post("DataFile/getdata", this.GetData, {
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
       GetData:{
        path: "",
        iduser: 24,
      },
      pathDirectory: "",
      GetAllData:{
        path: undefined,
      },
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