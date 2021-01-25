<template>
  <div>
    <p
      @click="moveDirectory(inx + 1)"
      style="display: inline"
      v-for="(item, inx) in this.$store.state.directory"
      :key="inx"
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

      for (var j = 0; j < cntpopdirectory; j++) {
        this.$store.state.directory.pop();
        // console.log(j);
      }
      this.$store.state.path = "";
      this.pathDirectory = "/uploads/";
      // console.log(cnt)
      for (var i = 1; i < cnt; i++) {
        this.pathDirectory += this.$store.state.directory[i];
        this.$store.state.path += this.$store.state.directory[i];
        // console.log(item)
      }
      // this.$store.state.path = this.pathDirectory;
      this.getData();
    //   console.log(this.pathDirectory);
    },
    getData() {
    //   console.log(this.pathDirectory);
      this.pathFile.path = "/uploads";
      if (this.pathDirectory == "/uploads/") {
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