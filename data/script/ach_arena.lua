--竞技场完成 计数器
x200010_g_scriptId = 200010


function x200010_OnCounter(uuid, eventId, result)

	--参考AchievementKeyTable.csv
	local achKey = 5
	if result == 0 then
		--胜利
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	end
	
	local strMsg = string.format("x200010_OnCounter uuid=%d, eventId=%d, result=%d", 
					uuid, eventId, result)
	LuaWriteLog(strMsg)
	
  	return
  	
end

