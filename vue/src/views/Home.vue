<template>
  <div class="container-fluid">
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
      <div class="col" style="background-color:white">
        <!-- v-for="(item, inx) in dataFile"
          :key="inx"
          :NameFile="item.nameFile"
          :Type="item.type" -->
        <div class="row" style="">
          <div class="col" style="border-bottom: rgb(206, 206, 206) 1px solid;">
            <Directory />
          </div>
          <div class="col" style="border-bottom: rgb(206, 206, 206) 1px solid;">
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

export default {
  components: {
    Menu,
    FileComponents,
    Directory,
    beforeDirectory,
  },
  methods: {
    getData() {
      this.pathFile.path = "/uploads";
      // console.log(this.$axios);
      if (this.$route.params.id == undefined) {
        this.pathFile.path = "/uploads";
      } else {
        this.pathFile.path += decodeURIComponent(this.$route.params.id);
        // console.log(decodeURIComponent(this.$route.params.id));
        // console.log(this.pathFile.path);
      }

      if (this.$route.params.id != undefined) {
        var str = this.$route.params.id.split("/");
        var mapstr = str.map((str) => "/" + str);
        // console.log(this.$route.params.id);
        // console.log(mapstr);

        // this.$store.state.directory.push(this.$router.id.split("/"))
        this.$store.state.directory = ["Home"];
        for (var i = 0; i < mapstr.length - 1; i++) {
          this.$store.state.directory.push(mapstr[i + 1]);
          this.$store.state.path += mapstr[i + 1];
          // console.log(this.$store.state.path);
        }
      }

      // console.log(this.$route.params.id);
      // console.log(this.pathFile.path);
      this.$axios
        .post("DataFile/getdata", this.pathFile)
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
    this.getData();
    this.$store.state.atDirectory = ["Home/"];
    // var f = this.getData();
    // console.log(this.dataFile.nameFile);
    window.onpopstate = function () {
      location.reload();
    };
  },

  data() {
    return {
      pathFile: {
        path: "/uploads",
      },
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
