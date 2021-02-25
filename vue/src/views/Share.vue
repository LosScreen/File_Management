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
            @AnClock="onClock"
          >
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
        <FileShareComponents> </FileShareComponents>
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
import FileShareComponents from "@/components/Share/FileShareComponents.vue";
import Directory from "@/components/DirectoryComponents.vue";
import beforeDirectory from "@/components/Share/ButtonDirectoryShareComponents.vue";
import Navbar from "@/components/NavbarComponents.vue";

export default {
  components: {
    Menu,
    FileShareComponents,
    Directory,
    beforeDirectory,
    Navbar,
  },
  methods: {
    getDataFile() {
 this.GetData.path = "/uploads";
      // this.GetData.iduser =localStorage.IdUser;
      // console.log(this.$axios);
      if (this.$route.params.id == undefined) {
        this.GetData.path = null;
      } else {
        this.GetData.path += decodeURIComponent(this.$route.params.id);
      }
      // console.log(this.getData);
      this.$axios
        .post("DataFileShare/getdata", this.GetData, {
          headers: {
            Authorization:
              "Bearer " + localStorage.Token,
          },
        })
        .then((response) => {
          this.$store.state.dataFile = response.data;
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
        });
    },

    onClock(value) {
      this.dataFile.push({ nameFile: value });
    },
  },
  created() {
    window.onpopstate = function () {
      console.log("event");
    };
  },
  mounted() {
    this.getDataFile();
    this.$store.state.atDirectory = ["Home/"];
    // var f = this.getData();
    // console.log(this.dataFile.nameFile);

    window.onpopstate = function () {
      location.reload();
    };
  },

  data() {
    return {
      GetData: {
        path: "",
        iduser: 24,
        token: "",
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
          Name_Menu: "My Drive",
        },
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
