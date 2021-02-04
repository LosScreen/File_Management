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
      // console.log(j);
      this.$store.state.path = "";
      this.pathDirectory = "/uploads";
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
    getData() {
      //   console.log(this.pathDirectory);
      this.pathFile.path = "/uploads";
      if (this.pathDirectory == "/uploads") {
        this.pathFile.path = "/uploads";
      } else {
        // console.log(this.pathDirectory);
        this.pathFile.path = this.pathDirectory;
        // console.log(this.pathFile.path);
      }
      this.$axios
        .post("DataFile/getdata", this.pathFile)
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
      pathDirectory: "",
      pathFile: {
        path: "/uploads",
      },
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
};
</script>

<style>
</style>