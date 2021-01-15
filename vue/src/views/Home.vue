<template>
  <div class="container-fluid">
    <div class="row" style="">
      <div class="col-2" style="background-color: red">
        <ul
          style="list-style-type: none; margin: 0; padding: 12px 0px 12px 0px"
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
      <div class="col" style="background-color: yellow">
        <!-- v-for="(item, inx) in dataFile"
          :key="inx"
          :NameFile="item.nameFile"
          :Type="item.type" -->
        <FileComponents> </FileComponents>
      </div>
      <button v-on:click="getData()">AAAAA</button>
      <p v-for="(item, inx) in $store.state.dataFile" :key="inx">
        {{ item.nameFile }}
      </p>
    </div>
  </div>
</template>

<script>
import Menu from "@/components/Menu.vue";
import FileComponents from "@/components/FileComponents.vue";

export default {
  components: {
    Menu,
    FileComponents
  },
  methods: {
    getData() {
      this.pathFile.path = "/uploads"
      // console.log(this.$axios);
     if(this.$store.state.path == ""){
        this.pathFile.path = "/uploads";
      }
      else{
        this.pathFile.path += "/" + this.$store.state.path;
      }
      console.log(this.pathFile);
      this.$axios
        .post("DataFile/getdata", this.pathFile)
        .then(response => {
          this.$store.state.dataFile = response.data;
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
        .catch(error => {
          console.error(error);
        });
    },

    onClock(value) {
      this.dataFile.push({ nameFile: value });
    }
  },
  mounted() {
    this.getData();
    // console.log(this.dataFile.nameFile);
  },

  data() {
    return {
      pathFile:{
        path:"/uploads",
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
          Name_Menu: "New Folder"
        },
        {
          Name_Menu: "Uploads"
        }
      ]
    };
  }
  // watch:{
  //   value(){
  //     this.name_Folder = this.name_Fo
  //   }
  // }
};
</script>
