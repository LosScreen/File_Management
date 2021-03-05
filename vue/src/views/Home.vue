<template>
  <div class="container-fluid">
    <Navbar />
    <!-- <div class="row" style="">
      <div class="col" style="">
        <Directory  />
      </div>
      <div class="col" style="text-align: right;">
        <beforeDirectory />
      </div>
    </div> -->
    <div class="row" style="">
      <div class="col-2" style="background-color: white; padding: 0 10px 0 0">
        <ul
          class="endLine"
          style="list-style-type: none; padding: 12px 0px 12px 0px"
        >
          <Menu
            v-for="(item, inx) in nameMenu"
            :key="inx"
            :name_Menu="item.Name_Menu"
            
          >
          <!-- @AnClock="onClock" -->
          </Menu>
        </ul>
      </div>
      <div class="col" style="background-color: white">
        <!-- v-for="(item, inx) in dataFile"
          :key="inx"
          :NameFile="item.nameFile"
          :Type="item.type" -->
        <div class="row" style="">
          <div class="col" style="border-bottom: rgb(206, 206, 206) 1px solid">
            <Directory />
          </div>
          <div class="col" style="border-bottom: rgb(206, 206, 206) 1px solid">
            <beforeDirectory />
          </div>
        </div>
        <FileComponents> </FileComponents>
      </div>
      <!-- <button v-on:click="getData()">AAAAA</button>
      <p v-for="(item, inx) in $store.state.dataFile" :key="inx">
        {{ item.nameFile }}
      </p> -->
    </div>
  </div>
</template>

<script>
import Menu from "@/components/Menu.vue";
import FileComponents from "@/components/FileComponents.vue";
import Directory from "@/components/DirectoryComponents.vue";
import beforeDirectory from "@/components/ButtonDirectoryComponents.vue";
import Navbar from "@/components/NavbarComponents.vue";

export default {
  components: {
    Menu,
    FileComponents,
    Directory,
    beforeDirectory,
    Navbar,
  },
  methods: {
    getAllData(){
      this.GetAllData.path = "/uploads/"
      // console.log(this.GetData);
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
      // console.log("response");
      // this.GetData.iduser =localStorage.IdUser;
      // console.log(this.$axios);
      if (this.$route.params.id == undefined) {
        this.GetData.path = "/uploads";
      } else {
        this.GetData.path += decodeURIComponent(this.$route.params.id);
        // console.log(decodeURIComponent(this.$route.params.id));
        // console.log(this.GetData.path);
      }

      if (this.$route.params.id != "/" + localStorage.Username) {
        var str = this.$route.params.id.split("/");
        var mapstr = str.map((str) => "/" + str);
        // console.log(this.$route.params.id);
        // console.log(localStorage.Username);
        // console.log(mapstr);

        // this.$store.state.directory.push(this.$router.id.split("/"))
        this.$store.state.directory = ["Home"];
        for (var i = 0; i < mapstr.length - 2; i++) {
          this.$store.state.directory.push(mapstr[i + 2]);
          this.$store.state.path += mapstr[i + 1];
          // console.log(this.$store.state.path);
        }
      }

      // console.log(this.$route.params.id);
      // console.log(this.GetData.path);
      // this.pathFile.path = "/uploads/awe";
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
          // console.log(this.$store.state.dataFile);
          // console.log(this.$store.state.dataFile)
          // console.log(this.$store.state.dataFile);
          // console.log(this.dataFile);
          // console.log(response.data);

          // response.data.forEach(element => {
          //   console.log(element);
          //   this.dataFile.nameFile = element.nameFile;
          // });

          // this.dataFile.nameFile = response.nameFile;
          // console.log(response);
          // console.log(response.data[0].nameFile);
        })
        .catch((error) => {
          console.error(error);
          console.log("error");
        });
    },

    // onClock(value) {
    //   this.dataFile.push({ nameFile: value });
    // },
  },
  created() {
    window.onpopstate = function () {
      console.log("event");
    };
  },
  mounted() {
    // console.log(this.$store.state.dataFile)
    if(localStorage.Token == ""){
      this.$router.push("/login");
    }
    else if (localStorage.Token != null) {
    this.$store.state.dataFile == null;
    this.getData();
    this.getAllData();
    this.$store.state.atDirectory = ["Home/"];
    // var f = this.getData();
    // console.log(this.dataFile.nameFile);
    window.onpopstate = function () {
      location.reload();
    };
    }
  },

  data() {
    return {
      GetData: {
        path: "",
        iduser: 24,
        token: localStorage.token,
      },
      GetAllData:{
        path: undefined,
      },
      pathFile: {
        path: "/uploads",
      },
      id: 23,
      // dataFile: [
      //   {
      //     iD: null,
      //     nameFile: "",
      //     path: "",
      //     type: "",
      //     file: "",
      //     filedata: ""
      //   }
      // ],
      nameMenu: [
        {
          Name_Menu: "New Folder",
        },
        {
          Name_Menu: "Uploads",
        },
        {
          Name_Menu: "Share"
        }
      ],
    };
  },
  // watch:{
  //   value(){
  //     this.name_Folder = this.name_Fo
  //   }
  // }
};
</script>
<style>
.endLine {
  border-bottom: rgb(206, 206, 206) 1px solid;
}
</style>
