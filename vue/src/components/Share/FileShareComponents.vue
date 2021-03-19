<template>
  <div class="row">
    <div class="col" v-for="(item, inx) in $store.state.dataFile" :key="inx">
      <!-- Test(item.nameFile, item.type) -->
      <div
        v-if="item.type != 'Folder'"
        @contextmenu.prevent="$refs.menu.open"
        @click.right="logFile(item)"
        @click.left="Preview(item.path,item.nameFile,item.idUser)"
        class="card Card-Box"
        style="width: 18rem"
      >
        <img
          style="border-radius: 20px 20px 0 0"
          v-if="item.type == 'image'"
          class="card-img-top Card-Image mx-auto"
          :src="item.wwwPath"
          alt="Card image cap"
        />
        <img
          style="border-radius: 20px 20px 0 0"
          v-if="item.type == 'video'"
          class="card-img-top Card-Image mx-auto"
          :src="item.wwwPath"
          alt="Card image cap"
        />
        <!-- <img 
          v-if="item.type == 'Folder'"
          @click="openFolder(item.nameFile, item.type)"
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/FolderDefault.png"
          alt="Card image cap"
        /> -->
        <img
          style="border-radius: 20px 20px 0 0"
          v-if="item.type == 'application'"
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/ImageDefault.png"
          alt="Card image cap"
        />
        <div class="card-body">
          {{ item.nameFile }}
          <a v-if="item.mainFolder != 0">Share</a>
          <!-- {{ item.type }} -->
        </div>
      </div>

      <div
        v-if="item.type == 'Folder'"
        @contextmenu.prevent="$refs.menu.open"
        @click.right="logFile(item)"
        @click="openFolder(item.nameFile, item.type, item.path)"
        class="card Card-Box"
        style="width: 18rem"
      >
        <img
          style="border-radius: 20px 20px 0 0"
          v-if="item.type == 'Folder'"
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/FolderDefault.png"
          alt="Card image cap"
        />
        <div class="card-body">
          {{ item.nameFile }}
          <a v-if="item.mainFolder != 0">Share</a>
          <!-- {{ item.type }} -->
        </div>
      </div>
      <!-- <div v-if="item.type == 'Folder'" class="card" style="width: 18rem">
        <img
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/FolderDefault.png"
          alt="Card image cap"
        />
        <div class="card-body">
          {{ item.nameFile }}
          {{ item.type }}
        </div>
      </div>
      <div v-if="item.type == 'application'" class="card" style="width: 18rem">
        <img
          class="card-img-top Card-Image mx-auto"
          src=""
          alt="Card image cap"
        />
        <div class="card-body">
          {{ item.nameFile }}
          {{ item.type }}
        </div>
      </div> -->
    </div>

    <vue-context ref="menu" style="height: fit-content; width: fit-content">
      <li v-if="dataFile.type == 'Folder'">
        <a
          href="#"
          @click.prevent="openFolder(dataFile.nameFile, dataFile.type,dataFile.path)"
          >Open
        </a>
      </li>
      <li v-if="dataFile.type == 'Folder'">
        <a href="#" @click.prevent="downloadFolder(dataFile)">Downloads</a>
      </li>
      <!-- <li v-if="dataFile.type == 'Folder'">
        <a href="#" @click.prevent="ModalShare = true">Share</a>
      </li> -->
      <!-- <li v-if="dataFile.type == 'Folder'">
        <a href="#" @click.prevent="removeFolder()">Remove Folder</a>
      </li> -->
      <li v-if="dataFile.type != 'Folder'">
        <a href="#" @click.prevent="Preview(dataFile.path,dataFile.nameFile)">Open</a>
      </li>
      <li v-if="dataFile.type != 'Folder'">
        <a href="#" @click.prevent="downloadFile(dataFile)">Downloads</a>
      </li>
      <!-- <li v-if="dataFile.type != 'Folder'">
        <a href="#" @click.prevent="ModalShare = true">Share</a>
      </li>
      <li v-if="dataFile.type != 'Folder'">
        <a href="#" @click.prevent="removeFile()">Remove File</a>
      </li> -->
    </vue-context>
    <Modal v-model="ModalShare" title="My first modal">
      <label>UserName</label>
      <input v-model="UserNameShare" /><br />
      <button v-on:click="Share(dataFile)">Yes</button>
    </Modal>



  </div>
</template>


<script>
import VueContext from "vue-context";

// import axios from "axios";

export default {
  components: {
    VueContext,
  },

  name: "FileComponents",
  props: {
    //    dataFile: [
    //     {
    //       iD: null,
    //       nameFile: "",
    //       path: "",
    //       type: "",
    //       file: "",
    //       filedata: "",
    //     },
    //   ],
  },
  methods: {
    Share(data){
      console.log(data);
      console.log(this.UsernameShare);
      this.$axios
          .post("DataFile/Share?username="+this.UserNameShare, data ,{
          headers: {
            Authorization:
              "Bearer " + localStorage.Token,
          },
        })
          .then((res) => {
            console.log(res.data);
            this.ModalShare = false;
          })
          .catch((error) => {
            console.log(error);
          });
    },
    logDirectory(data) {
      // console.log(data);

      var str = this.$route.params.id.split("/");
      // console.log(this.$route.params.id.split("/"));
      var mapstr = str.map((str) => "/" + str);

      this.$store.state.directory = ["Home"];
      for (var i = 2; i < mapstr.length; i++) {
        this.$store.state.directory.push(mapstr[i]);
      }
      this.$store.state.atDirectory = "/" + data;
      // console.log(this.$store.state.atDirectory);
     
    },
    downloadFolder(data) {
      this.dataFile = data;
      const str = this.dataFile.nameFile + "?path=" + this.dataFile.path;
      const FileDownload = require("js-file-download");
      this.$axios
        .get("DataFile/downloadFolderZip/" + str, { responseType: "blob" })
        .then((response) => {
          FileDownload(response.data, this.dataFile.nameFile + ".zip");
        })
        .catch((error) => {
          console.log(error);
        });
    },
    downloadFile(data) {
      this.dataFile = data;
      const str = this.dataFile.nameFile + "?path=" + this.dataFile.path;
      const FileDownload = require("js-file-download");
      this.$axios
        .get("DataFile/download/" + str, { responseType: "blob" })
        // responseType: 'blob'
        .then((response) => {
          FileDownload(response.data, this.dataFile.nameFile);
          // console.log(JSON.stringify(response.data));
        })
        .catch((error) => {
          console.log(error);
        });
    },
    Preview(dataPath,dataName,id) {
      console.log(id);
      var path = dataPath + "/" + dataName;
      window.open("/PreviewShare/"+encodeURIComponent(path), "_blank");
      console.log(path);
    },
    getPhoto(data) {
      this.dataFile = data;
      // console.log(data);
      if (this.dataFile.type == "image") {
        this.$axios
          .post("DataFile/GetPhoto", this.dataFile)
          .then((res) => {
            // console.log("Okay");
            // this.pathPhoto = res.data;
            console.log(res.data);
          })
          .catch((error) => {
            console.log(error);
          });
      } else {
        console.log("error");
      }
    },

    alert(e) {
      console.log(e);
    },
    logFile(data) {
      this.dataFile = data;
      // this.$store.state.directory.push("eiei");
      // console.log(this.dataFile);
    },

    removeFolder() {
      // if (this.dataFile.path == "/uploads"){
      //   this.dataFile.path == ""
      // }
      this.$axios
        .post("DataFile/deleteFolder", this.dataFile)
        .then(() => {
          // console.log("Okay");
          this.getData();
          alert("ลบโฟลเดอร์สำเร็จ");
        })
        .catch((error) => {
          console.log(error);
        });
    },
    removeFile() {
      this.$axios
        .post("DataFile/deleteFile", this.dataFile)
        .then(() => {
          // console.log("Okay");
          this.getData();
        })
        .catch((error) => {
          console.log(error);
          alert("ลบไฟล์สำเร็จ");
        });
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
      this.GetData.path = "/uploads";
      // this.GetData.iduser =localStorage.IdUser;
      // console.log(this.$axios);
      if (this.$route.params.id == undefined) {
        this.GetData.path = "/uploads/"+localStorage.Username;
      } else {
        this.GetData.path += decodeURIComponent(this.$route.params.id);
        // console.log(decodeURIComponent(this.$route.params.id));
        // console.log(this.GetData.path);
      }
      console.log(this.GetData);
      // console.log(this.GetData);
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
    openFolder(nameFile, type, path) {
      // console.log(nameFile,type);
      // this.logDirectory(nameFile);

      // if (type == "Folder" && this.$store.state.path == "") {
      //   this.$store.state.path = this.$store.state.dataFile.find(x => x.nameFile = nameFile).path + "/" + nameFile;
      //   console.log(this.$store.state.dataFile.find(x => x.nameFile = nameFile).path);
      //   console.log(this.$store.state.path.replace("/uploads",""));
      // } else if (type == "Folder") {
      //   this.$store.state.path += "/" + nameFile;
      // }
      // this.$router.push(
      //   "/Share/" + encodeURIComponent(this.$store.state.path.replace("/uploads",""))
      // );



      console.log(path);
      path += "/" + nameFile;

      console.log(path);
      var str = path.split("/");
      var mapstr = str.map((str) => "/" + str);

      console.log(mapstr.length);
      console.log(mapstr);
      var cnt = mapstr.length;
      this.$store.state.path = "";
      for (var i = 2; i < cnt; i++) {
        console.log(mapstr[i]);
        this.$store.state.path += mapstr[i];

      }
      console.log(this.$store.state.path);

      this.$router.push(
        "/Share/" + encodeURIComponent(this.$store.state.path)
      );


      // console.log(this.$route.params.id);
      this.logDirectory(nameFile);
      this.getData();
    },
  },
  mounted() {
    // console.log(dataFile);
    // this.getData();
  },
  data() {
    return {
      UserNameShare:"",
      ModalShare: false,
      GetAllData:{
        path: undefined,
      },
      GetData: {
        path: "",
        iduser: 24,
        token: localStorage.token,
      },
      pathPhoto: "",
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
          IdUser: ""
        },
      ],
    };
  },
};
</script>

<style>
@import "~vue-context/dist/css/vue-context.css";

.Active {
  display: inline-block !important;
}
.OnClickMenu {
  border: 1px solid black;
  position: absolute;
  display: none;
}
.Card-Image {
  height: 300px;
  width: 100%;
}
.Card-Box {
  margin: 10px 0px;
  cursor: pointer;
  transition: 0.5s;
  border-radius: 20px;
}
.Card-Box:hover {
  background: #ffffff;
  border-radius: 20px;
  box-shadow: 2px 4px 15px rgba(0, 0, 0, 0.2);
  transition: 0.2s;
  transform: scale(1.02);
}
</style>
