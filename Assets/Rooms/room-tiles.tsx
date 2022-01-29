<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.5" tiledversion="1.7.2" name="room-tiles" tilewidth="16" tileheight="16" tilecount="1024" columns="32">
 <image source="room-tiles.png" width="512" height="512"/>
 <tile id="9">
  <animation>
   <frame tileid="6" duration="1000"/>
   <frame tileid="8" duration="1000"/>
  </animation>
 </tile>
 <wangsets>
  <wangset name="room" type="corner" tile="-1">
   <wangcolor name="" color="#ff0000" tile="-1" probability="1"/>
   <wangcolor name="" color="#00ff00" tile="-1" probability="1"/>
   <wangtile tileid="0" wangid="0,1,0,1,0,1,0,1"/>
   <wangtile tileid="97" wangid="0,0,0,1,0,0,0,0"/>
   <wangtile tileid="98" wangid="0,0,0,1,0,1,0,0"/>
   <wangtile tileid="99" wangid="0,0,0,0,0,1,0,0"/>
   <wangtile tileid="129" wangid="0,1,0,1,0,0,0,0"/>
   <wangtile tileid="131" wangid="0,0,0,0,0,1,0,1"/>
   <wangtile tileid="161" wangid="0,1,0,0,0,0,0,0"/>
   <wangtile tileid="162" wangid="0,1,0,0,0,0,0,1"/>
   <wangtile tileid="163" wangid="0,0,0,0,0,0,0,1"/>
  </wangset>
 </wangsets>
</tileset>
