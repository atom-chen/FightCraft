--��ʯ�����¼� ������
x200003_g_scriptId = 200003


function x200003_OnCounter(uuid, eventId, count)
	
	--�ο�AchievementKeyTable.csv
	local achKey = 51

	if count > 0 then
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	end
	
	local strMsg = string.format("x200003_OnCounter uuid=%d, eventId=%d, count=%d", 
					uuid, eventId, count)
	LuaWriteLog(strMsg)
	
  	return
  	
end

