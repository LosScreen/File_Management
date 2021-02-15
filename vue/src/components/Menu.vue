<template>
  <div class="">
    <div v-if="name_Menu == 'New Folder'">
      <li class="button" style="" @click="ModalNewFolder = true">
        <i class="fas fa-folder-plus"></i>
        {{ name_Menu }}
      </li>
    </div>
    <div v-else-if="name_Menu == 'Uploads'">
      <li class="button" style="" @click="showModal = true">
        <i class="fas fa-plus"></i>
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
      formData.append("filedata", this.file);
      // formData.append("iduser", localStorage.IdUser);
      if (this.$store.state.path != "") {
        formData.append("path",decodeURIComponent(this.$route.params.id));
        console.log(this.$store.state.path);
      } else if (this.$store.state.path == "") {
        formData.append("path","/"+localStorage.Username);
      }
      // console.log(formData.path);

      for (var pair of formData.entries()) {
    console.log(pair[0]+ ', ' + pair[1]); 
}
      // this.fileData.filedata = formData;
      // this.fileData.path = this.$store.state.path;

      this.$axios
        .post("DataFile/putfile2", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
            Authorization:
              "Bearer " + localStorage.Token
          },
          onUploadProgress: function (progressEvent) {
            this.uploadPercentage = parseInt(
              Math.round((progressEvent.loaded / progressEvent.total) * 100)
            );
          }.bind(this),
        })
        .then(() => {
          this.getData();
          console.log("SUCCESS!!");
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    handleFileUpload() {
      // console.log(this.$refs.file.files[0]);
      this.file = this.$refs.file.files[0];
    },
    Test() {
      this.$emit("AnClock", this.name_log);
    },
    createFolderDate() {
      this.New_Folder.NameFile = this.inPutPath;
      this.New_Folder.IdUser = localStorage.IdUser;
      // console.log(this.$store.state.path);
      if (this.$store.state.path == "") {
        this.New_Folder.Path = "/uploads/"+localStorage.Username;
        console.log("!");
      } else if (this.$store.state.path != "") {
        this.New_Folder.Path = "/uploads"+ decodeURIComponent(this.$route.params.id);
        console.log("2");
      }
      console.log(this.$store.state.directory);
      if (this.$store.state.NameFile != "") {

        this.$axios
          .post("DataFile/createfolder", this.New_Folder, {
          headers: {
            Authorization:
              "Bearer " + localStorage.Token,
          },
        })
          .then(() => {
            this.getData();
          })
          .catch((error) => {
            console.error(error);
          });
      } else {
        alert("Name Folder is Null");
      }
    },
    getData() {
      this.GetData.path = "/uploads/"+localStorage.Username;
      // this.GetData.iduser =localStorage.IdUser;
      if (this.$store.state.path == "") {
        this.GetData.path = "/uploads/"+localStorage.Username;
      } else {
        this.GetData.path += this.$store.state.path;
        // console.log(this.pathFile.path);
      }
      this.GetData.iduser = localStorage.IdUser;
      console.log(this.GetData);
      // this.pathFile.path = "/uploads/asd/dsa";
      // console.log(this.pathFile.path);
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
      GetData:{
        path: "",
        iduser: 24,
      },
      New_Folder: {
        NameFile: "",
        Path: "/",
        IdUser:null,
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
  vertical-align: text-top;
  height: 40px;
  text-align: left;
  border-radius: 0 20px 20px 0;
  margin-bottom: 10px;
  padding: 8px 0 0 25px;
}

.button:hover {
  background-color: rgb(236, 236, 236);
}
</style>
