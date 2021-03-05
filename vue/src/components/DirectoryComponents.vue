<template>
  <div
    class=""
    style="
      display: flex;
      justify-content: left;
      height: 50px;
    "
  >
    <p
      @click="moveDirectory(inx + 1)"
      style="display: inline; margin: auto 3px;"
      v-for="(item, inx) in this.$store.state.directory"
      :key="inx"
      class="buttonDirectory"
    >
      {{ item }}
    </p>
  </div>
</template>

<script>
export default {
  methods: {
    moveDirectory(cnt) {
      // if(cnt == 1)
      const cntArr = this.$store.state.directory.length;
      // console.log(cntArr);
      // console.log(cnt);
      var cntpopdirectory = cntArr - cnt;

      console.log(cnt);
      for (var j = 0; j < cntpopdirectory; j++) {
        this.$store.state.directory.pop();
        // console.log(j);
      }

      var str = this.$route.params.id.split("/");
      var mapstr = str.map((str) => "/" + str);

      console.log(mapstr);
      

      this.$store.state.path = "";
      this.pathDirectory = "/uploads";
      console.log(this.$store.state.directory);
      for (var i = 0; i < cnt; i++) {
        this.pathDirectory += mapstr[i+1];
        this.$store.state.path += mapstr[i+1];
      }
      console.log(this.pathDirectory);
      console.log(this.$store.state.directory.length);
      // console.log(this.$store.state.directory.length);
      if (this.$store.state.directory.length >= 1) {
        this.$router.push(
          "/MyDrive/" +
            encodeURIComponent(this.pathDirectory.replace("/uploads", ""))
        );
        console.log(encodeURIComponent(this.pathDirectory.replace("/uploads", "")));
        console.log(this.pathDirectory);
      } else if (this.$store.state.directory.length == 0) {
        this.$router.push(
          "/MyDrive/" +
            encodeURIComponent(this.pathDirectory.replace("/uploads", ""))
        );
        console.log("eiei");
      }
      // console.log(this.pathDirectory)
      // this.$store.state.path = this.pathDirectory;
      this.getData();
      //   console.log(this.pathDirectory);
    },
    getData() {
      //   console.log(this.pathDirectory);
      this.pathFile.path = "/uploads/"+localStorage.Username;
      if (this.pathDirectory == "/uploads/"+localStorage.Username) {
        // this.GetData.path = "/uploads/"+localStorage.Username;
        // this.$router.push("/MyDrive");
        // console.log(this.GetData.path);
        this.GetData.path = this.pathDirectory;
      } else {
        // console.log(this.pathDirectory);
        // console.log(this.pathFile.path);
        this.GetData.path = this.pathDirectory;
        // console.log(this.GetData.path);
        // this.$router.push("/MyDrive/" + encodeURIComponent(this.pathDirectory.replace("/uploads", "")));
        // console.log(encodeURIComponent(this.pathDirectory.replace("/uploads/", "")));
        // console.log(decodeURIComponent(this.pathDirectory.replace("/uploads/", "")));
        // console.log(this.pathFile.path);
        //  this.pathFile.path = "/uploads/asd"
      }
      this.$axios
        .post("DataFile/getdata", this.GetData, {
          headers: {
            Authorization:
              "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.$store.state.dataFile = response.data;
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
.buttonDirectory {
  cursor: pointer;
  padding: 5px 10px;
  font-weight: bold;
  /* font-size: 20px; */
  /* top: 50% ; */
}
.buttonDirectory:hover {
  /* font-size: 20px; */
  background-color: rgba(175, 175, 175, 0.575);
  border-radius:10px;
}
</style>