--武将品质更新 计数器
x200005_g_scriptId = 200005


function x200005_OnCounter(uuid, eventId, quality,IsUp)
	--品质类型
	--enum QualityType
	--白
	--WhiteQuality	= 0;
	--绿
	--GreenQuality	= 1;
	--蓝
	--BlueQuality	= 2;
	--紫
	--PurpleQuality	= 3;
	--橙
	--OrangeQuality = 4;
	--金
	--GoldenQuality	= 5;
	--黑
	--BlackQuality	= 6;	


	--参考AchievementKeyTable.csv
	
	
	local achKey = 34
	
	if IsUp==0 then
		if quality >= 1 then
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
		if quality >= 2 then
		achKey = 35
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
		if quality >= 3 then
		achKey = 36
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
	elseif IsUp==1 then
		if quality >= 3 then
		achKey = 36
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		elseif quality >= 2 then
		achKey = 35
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		elseif quality >= 1 then
		achKey = 34
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
	end
			local strMsg = string.format("x200005_OnCounter uuid=%d, eventId=%d, quality=%d", 
					uuid, eventId, quality)
		LuaWriteLog(strMsg)
  	return 	
end

