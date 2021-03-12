<template>
  <div>
    Report
    <select v-model="selected">
      <!-- <option disabled value="">Please select one</option> -->
      <option>All</option>
      <option v-for="(data, inx) in this.userArry" :key="inx">{{
        data.userName
      }}</option>
      <option>C</option>
    </select>
    <span>Selected: {{ selected }}</span>
    <button @click="report">OK</button>
  </div>
</template>

<script>
export default {
  methods: {
    getdata() {
      this.$axios
        .get("api/report/getalluser")
        .then((res) => {
          this.userArry = res.data;
          console.log(res.data);
        })
        .catch((error) => {
          console.error(error);
          console.log("error");
        });
    },
    report() {
        const FileDownload = require("js-file-download");
        this.$axios
        .get("api/report/ReportExcel/"+ this.selected,{ responseType: "blob" })
        .then((res) => {
          FileDownload(res.data, this.selected+".xlsx","application/excel");
        })
        .catch((error) => {
          console.error(error);
        });
    },
  },

  mounted() {
    this.getdata();
  },

  data() {
    return {
      selected: "",
      userArry: [],
    };
  },
};
</script>

<style></style>
