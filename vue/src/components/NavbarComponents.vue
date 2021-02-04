<template>
  <div
    class="row w-100"
    style="background-color: white;text-align:left;border-bottom:1px gray solid;margin:0;padding: 10px; 0px"
  >
    <div class="col">
      <i class="fab fa-google-drive" style="font-size: 25px; margin-top: 10px">
        <div style="display: inline-block; width: 200px; margin: 0 0 0 10px">
          Piyapon Drive
        </div>
      </i>
      <div style="" class="input-box">
        <i class="fas fa-search" style="margin: 0 10px 0 0"></i>
        <input
          placeholder="ค้นหาที่ใช้ไม่ได้จริง"
          type="text"
          style=""
          class="input-search"
        />
      </div>
    </div>
    <div class="col" style="text-align: right; margin: 12px 0 0 0">
      <div class="d-inline-block user-box">
        <i class="fas fa-user" style="margin: 4px 0 0 0; color: white"></i>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Menu",
};
</script>

<style>
.input-box {
  display: inline-block;
  margin-left: 40px;
  background-color: rgb(228, 228, 228);
  border-radius: 10px;
  height: 60px;
  width: 470px;
  padding: 10px 0 0 25px;
}
.input-search {
  border-radius: 10px;
  height: 40px;
  width: 400px;
  border: none;
  background-color: transparent;
}
.user-box {
  height: 50px;
  width: 50px;
  border-radius: 25px;
  background-color: green;
  padding: 13px;
  margin: 0px 20px 0 0;
  text-align: center;
}
</style>