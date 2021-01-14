<template>
  <div>
    <div v-if="name_Menu == 'New Folder'">
      <li
        class="button"
        style="
          height: 50px;
          text-align: center;
          padding: 15px 0px 25px 0px;
          margin-bottom: 10px;
        "
        @click="ModalNewFolder = true"
      >
        {{ name_Menu }}
      </li>
    </div>
    <div v-else-if="name_Menu == 'Uploads'">
      <li
        class="button"
        style="
          height: 50px;
          text-align: center;
          padding: 15px 0px 25px 0px;
          margin-bottom: 10px;
        "
        @click="showModal = true"
      >
        {{ name_Menu }}
      </li>
    </div>

    <Modal v-model="ModalNewFolder" title="My first modal">
      <label>Name Folder</label>
      <input v-model="inPutPath" /><br />
      <button v-on:click="createFolderDate()">Yes</button>
    </Modal>
    <Modal v-model="showModal" title="My first modal">
      <label
        >File
        <input
          type="file"
          id="file"
          ref="file"
          v-on:change="handleFileUpload()"
        />
      </label>
      <br />
      <progress max="100" :value.prop="uploadPercentage"></progress>
      <br />
      <button v-on:click="submitFile()">Submit</button>
    </Modal>
  </div>
</template>

<script>
export default {
  components: {},
  methods: {
    submitFile() {
      let formData = new FormData();
      formData.append("file", this.file);
      this.fileData.filedata = formData;
      this.fileData.path = this.$store.state.path;

      this.$axios
        .post("DataFile/putfile2", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
          onUploadProgress: function (progressEvent) {
            this.uploadPercentage = parseInt(
              Math.round((progressEvent.loaded / progressEvent.total) * 100)
            );
          }.bind(this),
        })
        .then(function () {
          console.log("SUCCESS!!");
        })
        .catch(function () {
          console.log("FAILURE!!");
        });
    },
    handleFileUpload() {
      this.file = this.$refs.file.files[0];
    },
    Test() {
      this.$emit("AnClock", this.name_log);
    },
    createFolderDate() {
      this.New_Folder.NameFile = this.inPutPath;
      console.log(this.$store.state.path);
      if (this.$store.state.path == "") {
        this.New_Folder.Path = "/uploads";
      } else if (this.$store.state.path != "") {
        this.New_Folder.Path = "/uploads" + "/" + this.$store.state.path;
      }
      // console.log(this.New_Folder);
      this.$axios
        .post("DataFile/createfolder", this.New_Folder)
        .then(() => {
          this.getData();
        })
        .catch((error) => {
          console.error(error);
        });
    },
    getData() {
      if (this.$store.state.path == "") {
        this.pathFile.path = "/uploads";
      } else {
        this.pathFile.path = this.pathFile.path + "/" + this.$store.state.path;
        console.log(this.pathFile.path);
      }
      // console.log(this.pathFile.path);
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

  name: "Menu",
  props: {
    name_Menu: String,
  },
  data() {
    return {
      showModal: false,
      ModalNewFolder: false,
      inPutPath: "",
      pathFile: {
        path: "/uploads",
      },
      New_Folder: {
        NameFile: "",
        Path: "/",
      },
      file: "",
      uploadPercentage: 0,
      fileData: {
        filedata: "",
        path: "",
      },
      // dataFile: [
      //   {
      //     iD: null,
      //     NameFile: "",
      //     Path: "",
      //     Type: "",
      //     File: "",
      //     filedata: ""
      //   }
      // ]
    };
  },
};
</script>

<style>
.button {
  text-decoration: none;
  font-size: 16px;
  color: #000000;
  border: none;
  cursor: pointer;
  background-color: rgb(21, 180, 82);
  vertical-align: text-top;
}

.button:hover {
  background-color: rgb(16, 104, 50);
}
</style>
