--һ����ʯ
x100006_g_scriptId = 100006

--�¼�����
function x100006_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 1) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100006_OnAchievementProgress(uuid, reachNum)
  	return 1
end

