--������� ������
x200008_g_scriptId = 200008


function x200008_OnCounter(uuid, eventId)

	--�ο�AchievementKeyTable.csv
	local achKey = 15
	LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	
	local strMsg = string.format("x200008_OnCounter uuid=%d, eventId=%d", 
					uuid, eventId)
	LuaWriteLog(strMsg)
	
  	return
  	
end

