--��������¼� ������
x200002_g_scriptId = 200002


function x200002_OnCounter(uuid, eventId, count)
	
	--�ο�AchievementKeyTable.csv
	local achKey = 50

	if count > 0 then
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	end
	
	local strMsg = string.format("x200002_OnCounter uuid=%d, eventId=%d, count=%d", 
					uuid, eventId, count)
	LuaWriteLog(strMsg)
	
  	return
  	
end

