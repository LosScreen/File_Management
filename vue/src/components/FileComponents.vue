<template>
  <div class="row">
    <div class="col" v-for="(item, inx) in $store.state.dataFile" :key="inx">
      <div v-on:click="Test(item.nameFile,item.type)" class="card Card-Box" style="width: 18rem">
        <img
          v-if="item.type == 'image'"
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/ImageDefault.png"
          alt="Card image cap"
        />
        <img
          v-if="item.type == 'Folder'"
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/FolderDefault.png"
          alt="Card image cap"
        />
        <img
          v-if="item.type == 'application'"
          class="card-img-top Card-Image mx-auto"
          src="@/assets/folderIcon/ImageDefault.png"
          alt="Card image cap"
        />
        <div class="card-body">
          {{ item.nameFile }}
          {{ item.type }}
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
  </div>
</template>

<script>
export default {
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
    getData() {
      if(this.$store.state.path == ""){
        this.pathFile.path = "/uploads";
      }
      else{
        this.pathFile.path = this.pathFile.path + "/" + this.$store.state.path;
      }
      // console.log(this.pathFile.path);
      this.$axios
        .post("DataFile/getdata", this.pathFile)
        .then(response => {
          this.$store.state.dataFile = response.data;
          //   console.log(this.dataFile);
        })
        .catch(error => {
          console.error(error);
        });
    },
    Test(nameFile,type){
      // console.log(nameFile,type);
      if(type == 'Folder'){
      this.$store.state.path = nameFile;
      this.getData();
      }
      else{
        // console.log(nameFile,type);
      }
      // console.log(this.$store.state.path);
    }
  },
  mounted() {
    // console.log(dataFile);
    // this.getData();
  },
  data() {
    return {
      pathFile:{
        path:"/uploads"
      }
      // dataFile: [
      //   {
      //     iD: null,
      //     nameFile: "",
      //     path: "",
      //     type: "",
      //     file: "",
      //     filedata: ""
      //   }
      // ]
    };
  }
};
</script>

<style>
.Card-Image {
  height: 300px;
  width: 100%;
}
.Card-Box {
  margin: 10px 0px;
  cursor: pointer;
  transition: 0.5s;
}
.Card-Box:hover {
  background: #ffffff;
  border-radius: 10px;
  box-shadow: 2px 4px 15px rgba(0, 0, 0, 0.2);
  transition: 0.2s;
  transform: scale(1.02);
}
</style>
