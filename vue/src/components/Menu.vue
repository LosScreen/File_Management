<template>
  <div class="">
    <div v-if="name_Menu == 'New Folder'">
      <li class="button" style="" @click="ModalNewFolder = true">
        <i class="fas fa-folder-plus"></i>
        {{ name_Menu }}
      </li>
    </div>
    <div v-else-if="name_Menu == 'Uploads'">
      <li class="button" style="" @click="Upload = true">
        <i class="fas fa-plus"></i>
        {{ name_Menu }}
      </li>
    </div>
    <div v-if="name_Menu == 'Share'">
      <li class="button" style="" @click="ShareLink">
        <i class="fas fa-folder-plus"></i>
        {{ name_Menu }}
      </li>
    </div>
    <div v-if="name_Menu == 'My Drive'">
      <li class="button" style="" @click="MyDrive">
        <i class="fas fa-folder-plus"></i>
        {{ name_Menu }}
      </li>
    </div>
    

    <Modal v-model="ModalNewFolder" title="New Folder">
      <label style="display: inline-block;">Name Folder</label>
      <input style="display: inline-block; margin:0px 10px;" v-model="inPutPath" />
      <button style="display: inline-block;" v-on:click="createFolderDate()">Yes</button>
    </Modal>
    <Modal v-model="Upload" title="My first modal">
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
    MyDrive(){
      this.$router.push("/MyDrive/" +encodeURIComponent("/"+localStorage.Username));
      this.$store.state.directory = ["Home"]
    },
    ShareLink(){
      this.$router.push("/Share/");
      this.$store.state.directory = ["Home"]
    },
    submitFile() {
      // console.log(this.$store.state.dataFile.filter(item => item.nameFile == this.$refs.file.files[0].name).length);
      if (this.$store.state.dataFile.filter(item => item.nameFile == this.$refs.file.files[0].name).length == 0){
      let formData = new FormData();
      formData.append("filedata", this.file);
      // formData.append("iduser", localStorage.IdUser);
      if (this.$store.state.path != "") {
        formData.append("path",decodeURIComponent(this.$route.params.id));
        // console.log(this.$store.state.path);
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
          // console.log("SUCCESS!!");
          this.Upload = false;
        })
        .catch(function (error) {
          console.log(error);
        });
        }else if (this.$store.state.dataFile.filter(item => item.nameFile == this.$refs.file.files[0].name).length != 0){
          alert("อัพโหลดไม่ได้จ้า");
        }
    },
    handleFileUpload() {
      // console.log(this.$refs.file.files[0].name);
      this.file = this.$refs.file.files[0];
    },
    Test() {
      this.$emit("AnClock", this.name_log);
    },
    createFolderDate() {
      // var arr=this.$store.state.dataFile.filter(item => item.nameFile == this.inPutPath);
      // console.log(arr.length);
      if(this.$store.state.dataFile.filter(item => item.nameFile == this.inPutPath).length != 0){
        alert("สร้างไม่ได้จ้า")
      }else if (this.$store.state.dataFile.filter(item => item.nameFile == this.inPutPath).length == 0){
      // this.$store.state.dataFile.forEach(data => {
      //   // console.log(data.nameFile);
      // });
      // console.log(this.$store.state.dataFile);
      this.New_Folder.NameFile = this.inPutPath;
      // this.New_Folder.IdUser = localStorage.IdUser;
      // console.log(this.$store.state.path);
      if (this.$store.state.path == "") {
        this.New_Folder.Path = "/uploads/"+localStorage.Username;
        // console.log("!");
      } else if (this.$store.state.path != "") {
        this.New_Folder.Path = "/uploads"+ decodeURIComponent(this.$route.params.id);
        // console.log("2");
      }
      // console.log(this.$store.state.directory);
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
            this.ModalNewFolder = false;
          })
          .catch((error) => {
            console.error(error);
          });
      } else {
        alert("Name Folder is Null");
      }
      }
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
      this.GetData.path = "/uploads/"+localStorage.Username;
      // this.GetData.iduser =localStorage.IdUser;
      if (this.$store.state.path == "") {
        this.GetData.path = "/uploads/"+localStorage.Username;
      } else {
        this.GetData.path += this.$store.state.path;
        // console.log(this.pathFile.path);
      }
      // console.log(this.GetData);
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
          this.$store.state.defaultDataFile = response.data;
            // console.log(this.dataFile);
          this.getAllData();
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
      GetAllData:{
        path: undefined,
      },
      Testdata:{
        NameFile: "Test",
        Path: "/uploads/asd",
      },
      Upload: false,
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
