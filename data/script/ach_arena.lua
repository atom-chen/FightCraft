--��������� ������
x200010_g_scriptId = 200010


function x200010_OnCounter(uuid, eventId, result)

	--�ο�AchievementKeyTable.csv
	local achKey = 5
	if result == 0 then
		--ʤ��
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	end
	
	local strMsg = string.format("x200010_OnCounter uuid=%d, eventId=%d, result=%d", 
					uuid, eventId, result)
	LuaWriteLog(strMsg)
	
  	return
  	
end

